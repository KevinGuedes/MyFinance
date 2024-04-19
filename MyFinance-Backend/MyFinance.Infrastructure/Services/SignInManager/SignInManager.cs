using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MyFinance.Application.Abstractions.Services;
using MyFinance.Domain.Entities;
using System.Security.Claims;

namespace MyFinance.Infrastructure.Services.SignInManager;

public sealed class SignInManager : ISignInManager
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ITimeLimitedDataProtector _tldp;
    private readonly LockoutOptions _lockoutOptions;
    private readonly SignInOptions _signInOptions;

    public SignInManager(
        IHttpContextAccessor httpContextAccessor,
        IDataProtectionProvider idp,
        IOptions<SignInOptions> signInOptions)
    {
        _httpContextAccessor = httpContextAccessor;
        _signInOptions = signInOptions.Value;
        _lockoutOptions = signInOptions.Value.LockoutOptions;

        if (_signInOptions.MagicSignInTokenDurationInMinutes > 20)
            throw new ArgumentException("Magic sign in token duration cannot be greater than 20 minutes");
       
        _tldp = idp
            .CreateProtector(_signInOptions.MagicSignInTokenPurpose)
            .ToTimeLimitedDataProtector();

        ArgumentNullException.ThrowIfNull(
            _lockoutOptions.LockoutThresholds,
            nameof(_lockoutOptions.LockoutThresholds));

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
        => _httpContextAccessor.HttpContext!.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

    public bool CanSignIn(DateTime? lockoutEndOnUtc)
        => !lockoutEndOnUtc.HasValue || lockoutEndOnUtc < DateTime.UtcNow;

    public bool WillLockoutOnNextAttempt(int failedSignInAttempts)
    {
        var nextFailedSignInAttempts = failedSignInAttempts + 1;

        if (nextFailedSignInAttempts >= _lockoutOptions.UpperAttemptsThreshold)
            return true;

        if (_lockoutOptions.HasLockoutFor(nextFailedSignInAttempts))
            return true;

        return false;
    }

    public TimeSpan GetNextLockoutDuration(int failedSignInAttempts)
    {
        var nextFailedSignInAttempts = failedSignInAttempts + 1;

        if (nextFailedSignInAttempts >= _lockoutOptions.UpperAttemptsThreshold)
            return _lockoutOptions.UpperLockoutDurationThreshold;

        if (_lockoutOptions.HasLockoutFor(nextFailedSignInAttempts))
            return _lockoutOptions.GetLockoutDurationFor(nextFailedSignInAttempts);

        throw new InvalidOperationException("Lockout configuration not provided");
    }

    public bool ShouldLockout(int failedSignInAttempts)
        => failedSignInAttempts >= _lockoutOptions.UpperAttemptsThreshold ||
            _lockoutOptions.HasLockoutFor(failedSignInAttempts);

    public DateTime GetLockoutEndOnUtc(int failedSignInAttempts)
    {
        var hasReachUpperAttemtpsThreshold = failedSignInAttempts >= _lockoutOptions.UpperAttemptsThreshold;
        if (hasReachUpperAttemtpsThreshold)
            return DateTime.UtcNow.Add(_lockoutOptions.UpperLockoutDurationThreshold);

        if (_lockoutOptions.HasLockoutFor(failedSignInAttempts))
            return DateTime.UtcNow.Add(_lockoutOptions.GetLockoutDurationFor(failedSignInAttempts));

        throw new InvalidOperationException("Lockout configuration not provided");
    }

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
