using Microsoft.Extensions.Options;
using MyFinance.Application.Abstractions.Services;

namespace MyFinance.Infrastructure.Services.LockoutManager;

internal sealed class LockoutManager(IOptions<LockoutOptions> lockoutOptions) : ILockoutManager
{
    private readonly LockoutOptions _lockoutOptions = lockoutOptions.Value;

    public bool CanSignIn(DateTime? lockoutEndOnUtc)
        => !lockoutEndOnUtc.HasValue || lockoutEndOnUtc < DateTime.UtcNow;

    public bool ShouldLockout(int failedSignInAttempts)
        => HasLockoutFor(failedSignInAttempts);
 
    public DateTime GetLockoutEndOnUtc(int failedSignInAttempts)
        => DateTime.UtcNow.Add(GetLockoutDurationFor(failedSignInAttempts));

    public bool WillLockoutOnNextAttempt(int failedSignInAttempts)
        => HasLockoutFor(failedSignInAttempts + 1);

    public TimeSpan GetNextLockoutDurationFor(int failedSignInAttempts)
        => GetLockoutDurationFor(failedSignInAttempts + 1);

    public TimeSpan GetLockoutDurationFor(int failedSignInAttempts)
    {
        if(!HasLockoutFor(failedSignInAttempts))
            throw new InvalidOperationException("Lockout configuration not provided");

        if (failedSignInAttempts >= _lockoutOptions.UpperFailedAttemptsThreshold)
            return _lockoutOptions.UpperLockoutDurationThreshold;

        return _lockoutOptions.LockoutThresholds[failedSignInAttempts];
    }

    private bool HasLockoutFor(int failedSignInAttempts)
    {
        if (failedSignInAttempts >= _lockoutOptions.UpperFailedAttemptsThreshold)
            return true;

        return _lockoutOptions.FailedAttemptsThresholds.Contains(failedSignInAttempts);
    }
}
