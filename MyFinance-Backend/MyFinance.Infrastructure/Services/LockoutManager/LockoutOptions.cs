using MyFinance.Infrastructure.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace MyFinance.Infrastructure.Services.LockoutManager;

internal sealed class LockoutOptions : IValidatableOptions
{
    [Required]
    public IReadOnlyDictionary<int, TimeSpan> LockoutThresholds { get; set; }
        = new Dictionary<int, TimeSpan>
            {
                { 3, TimeSpan.FromMinutes(5) },
                { 6, TimeSpan.FromMinutes(10) },
                { 9, TimeSpan.FromHours(1) },
                { 12, TimeSpan.FromDays(1) },
            }.AsReadOnly();

    public int UpperFailedAttemptsThreshold => LockoutThresholds.Keys.Max();
    public TimeSpan UpperLockoutDurationThreshold => LockoutThresholds[UpperFailedAttemptsThreshold];
    public int[] FailedAttemptsThresholds => LockoutThresholds.Keys.ToArray();
}