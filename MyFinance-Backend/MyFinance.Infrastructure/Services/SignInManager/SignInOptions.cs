using MyFinance.Infrastructure.Abstractions;

namespace MyFinance.Infrastructure.Services.SignInManager;

internal sealed class SignInOptions : IValidatableOptions
{
    public readonly string MagicSignInTokenPurpose 
        = "MyFinance.Infrastructure.Services.SignInManager-MagicSignIn";
    public readonly int MaximumMagicSignInTokenDurationInMinutes = 20;

    public int MagicSignInTokenDurationInMinutes { get; set; } = 10;
    public TimeSpan MagicSignInTokenDuration => TimeSpan.FromMinutes(MagicSignInTokenDurationInMinutes);

    public void ValidateOptions()
    {
        if (MagicSignInTokenDurationInMinutes > MaximumMagicSignInTokenDurationInMinutes)
            throw new ArgumentException($"Magic sign in token duration cannot be greater than {MaximumMagicSignInTokenDurationInMinutes} minutes");
    }
}
