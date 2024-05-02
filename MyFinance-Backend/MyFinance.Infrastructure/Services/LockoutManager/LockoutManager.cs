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

    public bool WillLockoutOnNextAttempt(int failedSignInAttempts)
        => HasLockoutFor(failedSignInAttempts + 1);

    public TimeSpan GetNextLockoutDuration(int failedSignInAttempts)
    {
        var nextFailedSignInAttempts = failedSignInAttempts + 1;

        if (HasLockoutFor(nextFailedSignInAttempts))
            return GetLockoutDurationFor(nextFailedSignInAttempts);

        throw new InvalidOperationException("Lockout configuration not provided");
    }

    public DateTime GetLockoutEndOnUtc(int failedSignInAttempts)
    {
        if (HasLockoutFor(failedSignInAttempts))
            return DateTime.UtcNow.Add(GetLockoutDurationFor(failedSignInAttempts));

        throw new InvalidOperationException("Lockout configuration not provided");
    }

    private bool HasLockoutFor(int failedSignInAttempts)
    {
        if (failedSignInAttempts >= _lockoutOptions.UpperFailedAttemptsThreshold)
            return true;

        return _lockoutOptions.FailedAttemptsThresholds.Contains(failedSignInAttempts);
    }

    private TimeSpan GetLockoutDurationFor(int failedSignInAttempts)
    {
        if (failedSignInAttempts >= _lockoutOptions.UpperFailedAttemptsThreshold)
            return _lockoutOptions.UpperLockoutDurationThreshold;

        return _lockoutOptions.LockoutThresholds[failedSignInAttempts];
    }
}
