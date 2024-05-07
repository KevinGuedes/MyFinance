using MyFinance.Infrastructure.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace MyFinance.Infrastructure.Services.LockoutManager;

internal sealed class LockoutOptions : IValidatableOptions
{
    [Required]
    public IReadOnlyDictionary<int, TimeSpan> LockoutThresholds { get; set; }
        = new Dictionary<int, TimeSpan>
            {
                { 4, TimeSpan.FromMinutes(5) },
                { 7, TimeSpan.FromMinutes(10) },
                { 10, TimeSpan.FromHours(1) },
                { 13, TimeSpan.FromDays(1) },
            }.AsReadOnly();

    public int UpperFailedAttemptsThreshold => LockoutThresholds.Keys.Max();
    public int[] FailedAttemptsThresholds => LockoutThresholds.Keys.ToArray();
    public TimeSpan UpperLockoutDurationThreshold => LockoutThresholds[UpperFailedAttemptsThreshold];
}