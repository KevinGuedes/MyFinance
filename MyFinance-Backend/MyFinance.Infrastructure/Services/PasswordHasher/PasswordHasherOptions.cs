namespace MyFinance.Infrastructure.Services.PasswordHasher;

public sealed class PasswordHasherOptions
{
    public readonly int MinimumAllowedWorkFactor  = 14;
    public int WorkFactor { get; set; } = 16; 
}
