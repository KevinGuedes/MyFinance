using Azure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;
using MyFinance.Infrastructure.Extensions;
using System.Net.Http;
using System.Threading;

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
        options.SlidingExpiration = false;
        options.Cookie.IsEssential = true;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);

        options.Events.OnRedirectToLogin = async context =>
        {
            var unauthorizedProblemResponse = BuildUnauthorizedProblemResponse(context.HttpContext);
            context.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;

            await context.HttpContext.Response
                .WriteProblemResponseAsJsonAsync(unauthorizedProblemResponse);
        };

        options.Events.OnRedirectToAccessDenied = async context =>
        {
            var unauthorizedProblemResponse = BuildUnauthorizedProblemResponse(context.HttpContext);
            context.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;

            await context.HttpContext.Response
                .WriteProblemResponseAsJsonAsync(unauthorizedProblemResponse);
        };
    }

    private ObjectResult BuildUnauthorizedProblemResponse(HttpContext httpContext)
    {
        var problemDetails = _problemDetailsFactory.CreateProblemDetails(
            httpContext,
            detail: "User not logged in or doesn't have access to this resource",
            instance: httpContext.Request.Path,
            statusCode: StatusCodes.Status401Unauthorized);

        return new ObjectResult(problemDetails)
        {
            StatusCode = problemDetails!.Status
        };
    }
}
