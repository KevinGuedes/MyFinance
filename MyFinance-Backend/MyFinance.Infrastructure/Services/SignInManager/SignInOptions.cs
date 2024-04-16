namespace MyFinance.Infrastructure.Services.SignInManager;
public sealed class SignInOptions
{
    public LockoutOptions LockoutOptions { get; set; } = new LockoutOptions();
    public int TimeInMonthsToRequestPasswordUpdate { get; set; } = 6;
}
