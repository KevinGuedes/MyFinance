using MyFinance.Infrastructure.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace MyFinance.Infrastructure.Services.PasswordManager;

internal sealed class PasswordOptions : IValidatableOptions
{
    public readonly double SimilarityThreshold = 0.25;
    public readonly string[] ResetPasswordTokenPurpose = [
        "MyFinance.Infrastructure.Services.PasswordManager",
        "ResetPassword",
        "v1"
    ];

    [Required]
    [Range(14, int.MaxValue)]
    public int WorkFactor { get; set; } = 16;

    [Required]
    [Range(6, 18)]
    public int TimeInMonthsToRequestPasswordUpdate { get; set; } = 6;
}
