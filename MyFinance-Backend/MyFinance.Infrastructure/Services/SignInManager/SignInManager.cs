using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MyFinance.Application.Abstractions.Services;
using MyFinance.Domain.Entities;
using System.Security.Claims;
using System.Security.Cryptography;

namespace MyFinance.Infrastructure.Services.SignInManager;

internal sealed class SignInManager(
    IOptions<SignInOptions> signInOptions,
    IHttpContextAccessor httpContextAccessor,
    IDataProtectionProvider idp)
    : ISignInManager
{
    private readonly SignInOptions _signInOptions = signInOptions.Value;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly ITimeLimitedDataProtector _tldp = idp
        .CreateProtector(signInOptions.Value.MagicSignInTokenPurpose)
        .ToTimeLimitedDataProtector();

    public async Task SignInAsync(User user)
    {
        var claims = new List<Claim>
        {
            new("id", user.Id.ToString())
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);
        var authProperties = new AuthenticationProperties { IsPersistent = true };

        await _httpContextAccessor.HttpContext!.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal,
            authProperties);
    }

    public Task SignOutAsync()
        => _httpContextAccessor.HttpContext!
            .SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

    public string CreateMagicSignInToken(Guid magicSignInId)
        => _tldp.Protect(magicSignInId.ToString(), _signInOptions.MagicSignInTokenDuration);

    public bool TryGetMagicSignInIdFromToken(string token, out Guid magicSignInId)
    {
        try
        {
            var payload = _tldp.Unprotect(token);
            return Guid.TryParse(payload, out magicSignInId);
        }
        catch (CryptographicException)
        {
            magicSignInId = default;
            return false;
        }
    }
}
