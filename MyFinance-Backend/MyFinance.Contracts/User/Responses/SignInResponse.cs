namespace MyFinance.Contracts.User.Responses;

public sealed class SignInResponse(bool shouldUpdatePassword)
{
    public bool ShouldUpdatePassword { get; init; } = shouldUpdatePassword;
}
