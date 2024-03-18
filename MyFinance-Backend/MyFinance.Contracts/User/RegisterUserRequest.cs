namespace MyFinance.Contracts.User;

public sealed record RegisterUserRequest(
    string Name,
    string Email,
    string PlainTextPassword,
    string PlainTextConfirmationPassword);
