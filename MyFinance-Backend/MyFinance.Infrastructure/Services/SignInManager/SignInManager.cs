﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MyFinance.Application.Abstractions.Services;
using MyFinance.Domain.Entities;
using System.Security.Claims;

namespace MyFinance.Infrastructure.Services.SignInManager;

internal sealed class SignInManager : ISignInManager
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ITimeLimitedDataProtector _tldp;
    private readonly SignInOptions _signInOptions;

    public SignInManager(
        IHttpContextAccessor httpContextAccessor,
        IDataProtectionProvider idp,
        IOptions<SignInOptions> signInOptions)
    {
        _httpContextAccessor = httpContextAccessor;
        _signInOptions = signInOptions.Value;

        if (_signInOptions.MagicSignInTokenDurationInMinutes > 20)
            throw new ArgumentException("Magic sign in token duration cannot be greater than 20 minutes");
       
        _tldp = idp
            .CreateProtector(_signInOptions.MagicSignInTokenPurpose)
            .ToTimeLimitedDataProtector();

        ArgumentNullException.ThrowIfNull(
            _signInOptions.TimeInMonthsToRequestPasswordUpdate,
            nameof(_signInOptions.TimeInMonthsToRequestPasswordUpdate));
    }

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

    public bool ShouldUpdatePassword(DateTime lastPasswordUpdateOnUtc)
        => DateTime.UtcNow > lastPasswordUpdateOnUtc.AddMonths(_signInOptions.TimeInMonthsToRequestPasswordUpdate);

    public string CreateMagicSignInToken(Guid magicSignInId)
        => _tldp
            .CreateProtector(_signInOptions.MagicSignInTokenPurpose)
            .Protect(magicSignInId.ToString(), _signInOptions.MagicSignInTokenDuration);

    public bool TryGetMagicSignInIdFromToken(string token, out Guid magicSignInId)
    {
        try
        {
            var payload = _tldp
                .CreateProtector(_signInOptions.MagicSignInTokenPurpose)
                .Unprotect(token);

            if (Guid.TryParse(payload, out var magicSignInIdFromToken))
            {
                magicSignInId = magicSignInIdFromToken;
                return true;
            }

            magicSignInId = default;
            return false;
        }
        catch
        {
            magicSignInId = default;
            return false;
        }
    }
}
