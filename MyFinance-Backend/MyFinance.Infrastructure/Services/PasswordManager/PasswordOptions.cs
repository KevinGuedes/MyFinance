namespace MyFinance.Infrastructure.Services.PasswordManager;

internal sealed class PasswordOptions
{
    public readonly string ResetPasswordTokenPurpose 
        = "MyFinance.Infrastructure.Services.PasswordManager-ResetPassword";
    public readonly int MinimumAllowedWorkFactor = 14;
    public readonly double SimilarityThreshold = 0.25;

    public int WorkFactor { get; set; } = 16;
}
