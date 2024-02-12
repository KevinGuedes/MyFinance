using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace MyFinance.Application.Services.Auth;

public sealed class AuthService(IHttpContextAccessor httpContextAccessor) : IAuthService
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public async Task SignInAsync(string userEmail)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Email, userEmail),
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);
        var authProperties = new AuthenticationProperties { IsPersistent = true };

        await _httpContextAccessor.HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                authProperties);
    }

    public Task SignOutAsync()
        => _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
}
