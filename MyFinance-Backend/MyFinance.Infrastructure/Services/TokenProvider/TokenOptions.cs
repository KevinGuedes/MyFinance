using MyFinance.Infrastructure.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace MyFinance.Infrastructure.Services.TokenProvider;

internal sealed class TokenOptions : IValidatableOptions
{
    private readonly static string[] DefaultPurposes = [
        "MyFinance.Infrastructure.Services.TokenProvider",
        "v1"
    ];

    public readonly string[] ConfirmRegistrationTokenPurposes
        = ["ConfirmRegistration", .. DefaultPurposes];

    public readonly string[] MagicSignInTokenPurposes
        = ["MagicSignIn", .. DefaultPurposes];

    public readonly string[] ResetPasswordTokenPurposes
        = ["ResetPassword", .. DefaultPurposes];

    [Required]
    [Range(5, 15)]
    public int DefaultTokenDurationInMinutes { get; set; } = 15;
}
