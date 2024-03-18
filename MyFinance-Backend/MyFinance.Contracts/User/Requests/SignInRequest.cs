namespace MyFinance.Contracts.User.Requests;

public sealed class SignInRequest
{
    public required string Email { get; init; }
    public required string PlainTextPassword { get; init; }
};
