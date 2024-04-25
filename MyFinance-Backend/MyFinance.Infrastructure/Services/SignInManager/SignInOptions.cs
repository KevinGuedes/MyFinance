using MyFinance.Infrastructure.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace MyFinance.Infrastructure.Services.SignInManager;

internal sealed class SignInOptions : IValidatableOptions
{
    public readonly string[] MagicSignInTokenPurpose = [
        "MyFinance.Infrastructure.Services.SignInManager",
        "MagicSignIn",
        "v1"
    ];

    [Required]
    [Range(5, 15)]
    public int MagicSignInTokenDurationInMinutes { get; set; } = 10;

    public TimeSpan MagicSignInTokenDuration => TimeSpan.FromMinutes(MagicSignInTokenDurationInMinutes);
}
