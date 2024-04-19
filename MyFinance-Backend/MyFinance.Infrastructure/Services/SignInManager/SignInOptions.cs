namespace MyFinance.Infrastructure.Services.SignInManager;

public sealed class SignInOptions
{
    public readonly string MagicSignInTokenPurpose = "MagicSignIn";

    public LockoutOptions LockoutOptions { get; set; } = new LockoutOptions();
    public int TimeInMonthsToRequestPasswordUpdate { get; set; } = 6;
    public int MagicSignInTokenDurationInMinutes { get; set; } = 10;
    public TimeSpan MagicSignInTokenDuration
        => TimeSpan.FromMinutes(MagicSignInTokenDurationInMinutes);
}
