namespace MyFinance.Infrastructure.Services.PasswordManager;

public sealed class PasswordManagerOptions
{
    public readonly int MinimumAllowedWorkFactor = 14;
    public readonly double SimilarityThreshold = 0.25;
    public int WorkFactor { get; set; } = 16;
}
