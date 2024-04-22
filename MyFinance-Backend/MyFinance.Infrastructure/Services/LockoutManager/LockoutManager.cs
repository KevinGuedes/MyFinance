using Microsoft.Extensions.Options;
using MyFinance.Application.Abstractions.Services;

namespace MyFinance.Infrastructure.Services.LockoutManager;

internal sealed class LockoutManager : ILockoutManager
{
    private readonly LockoutOptions _lockoutOptions;

    public LockoutManager(IOptions<LockoutOptions> lockoutOptions)
    {
        _lockoutOptions = lockoutOptions.Value;

        if (_lockoutOptions.LockoutThresholds.Count is 0)
            throw new ArgumentException("Lockout thresholds must be provided");
    }

    public bool CanSignIn(DateTime? lockoutEndOnUtc)
        => !lockoutEndOnUtc.HasValue || lockoutEndOnUtc < DateTime.UtcNow;

    public bool ShouldLockout(int failedSignInAttempts)
        => failedSignInAttempts >= _lockoutOptions.UpperAttemptsThreshold ||
            _lockoutOptions.HasLockoutFor(failedSignInAttempts);

    public bool WillLockoutOnNextAttempt(int failedSignInAttempts)
    {
        var nextFailedSignInAttempts = failedSignInAttempts + 1;

        if (nextFailedSignInAttempts >= _lockoutOptions.UpperAttemptsThreshold)
            return true;

        return _lockoutOptions.HasLockoutFor(nextFailedSignInAttempts);
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

    public DateTime GetLockoutEndOnUtc(int failedSignInAttempts)
    {
        var hasReachUpperAttemtpsThreshold
            = failedSignInAttempts >= _lockoutOptions.UpperAttemptsThreshold;

        if (hasReachUpperAttemtpsThreshold)
            return DateTime.UtcNow.Add(_lockoutOptions.UpperLockoutDurationThreshold);

        if (_lockoutOptions.HasLockoutFor(failedSignInAttempts))
            return DateTime.UtcNow.Add(_lockoutOptions.GetLockoutDurationFor(failedSignInAttempts));

        throw new InvalidOperationException("Lockout configuration not provided");
    }
}
