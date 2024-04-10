using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;
using MyFinance.Infrastructure.Extensions;

namespace MyFinance.Infrastructure.Services.Auth;

internal class CookieConfiguration(ProblemDetailsFactory problemDetailsFactory)
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
            var unauthorizedProblemResponse = BuildUnauthorizedProblemResponse(context.HttpContext);
            context.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;

            await context
                .HttpContext
                .Response
                .WriteAsProblemPlusJsonAsync(unauthorizedProblemResponse);
        };

        options.Events.OnRedirectToAccessDenied = async context =>
        {
            var unauthorizedProblemResponse = BuildUnauthorizedProblemResponse(context.HttpContext);
            context.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;

            await context
                .HttpContext
                .Response
                .WriteAsProblemPlusJsonAsync(unauthorizedProblemResponse);
        };
    }

    private ObjectResult BuildUnauthorizedProblemResponse(HttpContext httpContext)
    {
        var problemDetails = _problemDetailsFactory.CreateProblemDetails(
            httpContext,
            detail: "User not logged in or doesn't have access to this resource",
            instance: httpContext.Request.Path,
            statusCode: StatusCodes.Status401Unauthorized);

        return new(problemDetails)
        {
            StatusCode = problemDetails!.Status
        };
    }
}
