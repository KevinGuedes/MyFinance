using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyFinance.Infrastructure.Common;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace MyFinance.IntegrationTests.Common;

internal class TestAuthenticationHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder,
    Guid userId) : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    public static string TestSchemeName => "TestScheme";

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var claims = new List<Claim>()
        {
            new(CustomClaimTypes.Id, userId.ToString()),
            new(CustomClaimTypes.SecurityStamp, Guid.NewGuid().ToString()),
        };

        var identity = new ClaimsIdentity(claims, "Test");
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, TestSchemeName);

        var result = AuthenticateResult.Success(ticket);

        return Task.FromResult(result);
    }
}
