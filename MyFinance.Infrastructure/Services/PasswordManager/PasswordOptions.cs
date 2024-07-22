using MyFinance.Infrastructure.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace MyFinance.Infrastructure.Services.PasswordManager;

internal sealed class PasswordOptions : IValidatableOptions
{
    public readonly double CosineSimilarityThreshold = 0.25;
    public readonly double JaroWinklerSimilarityThreshold = 0.45;

    [Required]
    [Range(14, int.MaxValue)]
    public int WorkFactor { get; set; } = 14;

    [Required]
    [Range(6, 18)]
    public int TimeInMonthsToRequestPasswordUpdate { get; set; } = 6;

    [Required]
    public bool IsHashingAlgorithmUpToDate { get; set; }
}
