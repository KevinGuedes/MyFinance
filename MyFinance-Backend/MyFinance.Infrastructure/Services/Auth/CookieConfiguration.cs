using Azure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;
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
            var unauthorizedResponse = BuildUnauthorizedResponse(context);

            await context.HttpContext.Response.WriteAsJsonAsync(
                value: unauthorizedResponse.Value,
                type: unauthorizedResponse.Value!.GetType(),
                options: null,
                contentType: "application/problem+json");
        };

        options.Events.OnRedirectToAccessDenied = async context =>
        {
            var unauthorizedResponse = BuildUnauthorizedResponse(context);

            await context.HttpContext.Response.WriteAsJsonAsync(
                value: unauthorizedResponse.Value,
                type: unauthorizedResponse.Value!.GetType(),
                options: null,
                contentType: "application/problem+json");
        };
    }

    private ObjectResult BuildUnauthorizedResponse(RedirectContext<CookieAuthenticationOptions> context)
    {
        var problemDetails = _problemDetailsFactory.CreateProblemDetails(
            context.HttpContext,
            instance: context.HttpContext.Request.Path,
            statusCode: StatusCodes.Status401Unauthorized);

        return new ObjectResult(problemDetails)
        {
            StatusCode = problemDetails!.Status
        };
    }
}
