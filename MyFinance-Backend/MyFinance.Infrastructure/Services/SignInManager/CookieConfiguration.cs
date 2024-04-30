using Microsoft.AspNetCore.Authentication;
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

internal sealed class CookieConfiguration(ProblemDetailsFactory problemDetailsFactory)
    : IConfigureNamedOptions<CookieAuthenticationOptions>
{
    private readonly ProblemDetailsFactory _problemDetailsFactory = problemDetailsFactory;

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
            var serviceProvider = context.HttpContext.RequestServices;
            var signInManager = serviceProvider.GetRequiredService<ISignInManager>();
            var userRepository = serviceProvider.GetRequiredService<IUserRepository>();
            
            var isSecurityStampValid = await ValidateSecurityStamp(context, userRepository);

            if (!isSecurityStampValid)
            {
                context.RejectPrincipal();
                await signInManager.SignOutAsync();
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
        CookieValidatePrincipalContext context, IUserRepository userRepository)
    {
        if(context.Principal is null)
            return false;

        if(TryGetUserId(context.Principal, out var userId) && 
            TryGetSecutiryStamp(context.Principal, out var securityStamp))
        {
            var user = await userRepository.GetByIdAsync(userId, default);
            
            return user is not null && user.SecurityStamp == securityStamp;
        }

        return false;
    }

    private static bool TryGetUserId(ClaimsPrincipal principal, out Guid userId)
    {
        var userIdClaim = principal.FindFirstValue("id");

        if(userIdClaim is null)
        {
            userId = Guid.Empty;
            return false;
        }

        return Guid.TryParse(userIdClaim, out userId);
    }

    private static bool TryGetSecutiryStamp(ClaimsPrincipal principal, out Guid securityStamp)
    {
        var securityStampClaim = principal.FindFirstValue("security-stamp");

        if(securityStampClaim is null)
        {
            securityStamp = Guid.Empty;
            return false;
        }

        return Guid.TryParse(securityStampClaim, out securityStamp);
    }
}
