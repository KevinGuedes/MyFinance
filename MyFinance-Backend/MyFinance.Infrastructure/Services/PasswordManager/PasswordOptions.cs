using MyFinance.Infrastructure.Abstractions;

namespace MyFinance.Infrastructure.Services.PasswordManager;

internal sealed class PasswordOptions : IValidatableOptions
{
    public readonly double SimilarityThreshold = 0.25;
    public readonly int MinimumAllowedWorkFactor = 14;
    public readonly int MaximumAllowedTimeInMonthsToRequestPasswordUpdate = 12;
    public readonly string[] ResetPasswordTokenPurpose = [
        "MyFinance.Infrastructure.Services.PasswordManager",
        "ResetPassword",
        "v1"
    ];

    public int WorkFactor { get; set; } = 16;
    public int TimeInMonthsToRequestPasswordUpdate { get; set; } = 6;

    public void ValidateOptions()
    {
        if (WorkFactor < MinimumAllowedWorkFactor)
            throw new ArgumentException($"Work factor must be equal or greater than {MinimumAllowedWorkFactor}");

        if (TimeInMonthsToRequestPasswordUpdate > MaximumAllowedTimeInMonthsToRequestPasswordUpdate)
            throw new ArgumentException($"Time in months to request password update must be equal or less than {MaximumAllowedTimeInMonthsToRequestPasswordUpdate}");
    }
}
