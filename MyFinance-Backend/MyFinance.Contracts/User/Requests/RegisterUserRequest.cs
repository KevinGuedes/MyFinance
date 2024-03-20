namespace MyFinance.Contracts.User.Requests;

public sealed class RegisterUserRequest
{
    public required string Name { get; init; }
    public required string Email { get; init; }
    public required string PlainTextPassword { get; init; }
    public required string PlainTextConfirmationPassword { get; init; }
}