namespace MyFinance.Infrastructure.Services.PasswordManager;

internal sealed class PasswordOptions
{
    public readonly string ResetPasswordTokenPurpose 
        = "MyFinance.Infrastructure.Services.PasswordManager-ResetPassword";
    public readonly double SimilarityThreshold = 0.25;
    public readonly int MinimumAllowedWorkFactor = 14;
    public readonly int MaximumAllowedTimeInMonthsToRequestPasswordUpdate = 12;

    public int WorkFactor { get; set; } = 16;
    public int TimeInMonthsToRequestPasswordUpdate { get; set; } = 6;
}
