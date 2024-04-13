﻿namespace MyFinance.Infrastructure.Services.SignInManager;

public sealed class LockoutOptions
{
    public IReadOnlyDictionary<int, TimeSpan> LockoutThresholds { get; set; }
    public int UpperAttemptsThreshold => LockoutThresholds.Keys.Max();
    public TimeSpan UpperLockoutDurationThreshold => LockoutThresholds[UpperAttemptsThreshold];

    public LockoutOptions()
    {
        LockoutThresholds = new Dictionary<int, TimeSpan>
        {
            { 3, TimeSpan.FromMinutes(5) },
            { 6, TimeSpan.FromMinutes(10) },
            { 9, TimeSpan.FromHours(1) },
            { 12, TimeSpan.FromDays(1) },
        }.AsReadOnly();
    }

    public bool HasLockoutFor(int failedSignInAttempts)
        => LockoutThresholds.ContainsKey(failedSignInAttempts);
}
