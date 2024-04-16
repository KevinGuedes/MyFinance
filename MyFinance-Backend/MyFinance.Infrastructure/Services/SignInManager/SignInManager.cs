using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MyFinance.Application.Abstractions.Services;
using MyFinance.Domain.Entities;
using System.Security.Claims;

namespace MyFinance.Infrastructure.Services.SignInManager;

public sealed class SignInManager : ISignInManager
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly LockoutOptions _lockoutOptions;
    private readonly SignInOptions _signInOptions;

    public SignInManager(IHttpContextAccessor httpContextAccessor, IOptions<SignInOptions> signInOptions)
    {
        _httpContextAccessor = httpContextAccessor;
        _signInOptions = signInOptions.Value;
        _lockoutOptions = signInOptions.Value.LockoutOptions;

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

    public bool CanSignIn(DateTime? lockoutEnd)
        => !lockoutEnd.HasValue || lockoutEnd < DateTime.UtcNow;

    public bool WillLockoutOnNextAttempt(int failedSignInAttempts)
    {
        var nextFailedSignInAttempts = failedSignInAttempts + 1;
       
        if(nextFailedSignInAttempts >= _lockoutOptions.UpperAttemptsThreshold)
            return true;

        if (_lockoutOptions.HasLockoutFor(nextFailedSignInAttempts))
            return true;

        return false;
    }

    public TimeSpan GetNextLockoutDuration(int failedSignInAttempts)
    {
        var nextFailedSignInAttempts = failedSignInAttempts + 1;

        if(nextFailedSignInAttempts >= _lockoutOptions.UpperAttemptsThreshold)
            return _lockoutOptions.UpperLockoutDurationThreshold;

        if (_lockoutOptions.HasLockoutFor(nextFailedSignInAttempts))
            return _lockoutOptions.LockoutThresholds[nextFailedSignInAttempts];

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

        if(_lockoutOptions.HasLockoutFor(failedSignInAttempts))
            return DateTime.UtcNow.Add(_lockoutOptions.LockoutThresholds[failedSignInAttempts]);
        
        throw new InvalidOperationException("Lockout configuration not provided");
    }

    public bool ShouldUpdatePassword(DateTime lastPasswordUpdate)
    {
        var utcNow = DateTime.UtcNow;
        var timeToLastPasswordUpdateInMonths =
            utcNow.Year - lastPasswordUpdate.Year * 12 + utcNow.Month - lastPasswordUpdate.Month;

        return timeToLastPasswordUpdateInMonths >= _signInOptions.TimeInMonthsToRequestPasswordUpdate;
    }
}
