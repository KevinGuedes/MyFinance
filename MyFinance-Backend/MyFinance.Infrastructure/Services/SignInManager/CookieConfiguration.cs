using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Application.Abstractions.Services;
using MyFinance.Contracts.Common;
using MyFinance.Infrastructure.Extensions;
using System.Security.Claims;

namespace MyFinance.Infrastructure.Services.SignInManager;

internal sealed class CookieConfiguration(
    ProblemDetailsFactory problemDetailsFactory,
    IServiceScopeFactory serviceScopeFactory)
    : IConfigureNamedOptions<CookieAuthenticationOptions>
{
    private readonly ProblemDetailsFactory _problemDetailsFactory = problemDetailsFactory;
    private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;

    public void Configure(string? name, CookieAuthenticationOptions options)
        => Configure(options);

    public void Configure(CookieAuthenticationOptions options)
    {
        options.Cookie.Name = "MF-Access-Token";
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Strict;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.IsEssential = true;
        options.SlidingExpiration = false;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);

        options.Events.OnRedirectToLogin = async context =>
        {
            var unauthorizedProblemResponse = BuildProblemResponse(
                context.HttpContext,
                StatusCodes.Status401Unauthorized,
                "User not signed in");

            context.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;

            await context
                .HttpContext
                .Response
                .WriteAsProblemPlusJsonAsync(unauthorizedProblemResponse);
        };

        options.Events.OnRedirectToAccessDenied = async context =>
        {
            var unauthorizedProblemResponse = BuildProblemResponse(
                context.HttpContext,
                StatusCodes.Status403Forbidden,
                "Access to resource has been denied");

            context.HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;

            await context
                .HttpContext
                .Response
                .WriteAsProblemPlusJsonAsync(unauthorizedProblemResponse);
        };

        options.Events.OnValidatePrincipal = async context =>
        {
            using IServiceScope scope = _serviceScopeFactory.CreateScope();

            var isSecurityStampValid = await ValidateSecurityStamp(context.Principal, scope);
            if (!isSecurityStampValid)
            {
                var signInManager = scope.ServiceProvider.GetRequiredService<ISignInManager>();
                await signInManager.SignOutAsync();
                context.RejectPrincipal();
            }
        };
    }

    private ObjectResult BuildProblemResponse(HttpContext httpContext, int statusCode, string detail)
    {
        var problemDetails = _problemDetailsFactory.CreateProblemDetails(
            httpContext,
            detail: detail,
            instance: httpContext.Request.Path,
            statusCode: statusCode);

        return new(new ProblemResponse(problemDetails))
        {
            StatusCode = problemDetails!.Status
        };
    }

    private static async Task<bool> ValidateSecurityStamp(
       ClaimsPrincipal? claimsPrincipal, IServiceScope scope)
    {
        var currentUserProvider = scope.ServiceProvider.GetRequiredService<ICurrentUserProvider>();

        if(claimsPrincipal is null)
            return false;

        if(currentUserProvider.TryGetCurrentUserId(claimsPrincipal, out var userId) && 
            currentUserProvider.TryGetCurrentUserSecurityStamp(claimsPrincipal, out var securityStamp))
        {
            var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
            var user = await userRepository.GetByIdAsync(userId, default);
            return user is not null && user.SecurityStamp == securityStamp;
        }

        return false;
    }
}
