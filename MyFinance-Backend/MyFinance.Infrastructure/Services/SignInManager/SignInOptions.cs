namespace MyFinance.Infrastructure.Services.SignInManager;

internal sealed class SignInOptions
{
    public readonly string MagicSignInTokenPurpose 
        = "MyFinance.Infrastructure.Services.SignInManager-MagicSignIn";

    public int TimeInMonthsToRequestPasswordUpdate { get; set; } = 6;
    public int MagicSignInTokenDurationInMinutes { get; set; } = 10;
    public TimeSpan MagicSignInTokenDuration
        => TimeSpan.FromMinutes(MagicSignInTokenDurationInMinutes);
}
