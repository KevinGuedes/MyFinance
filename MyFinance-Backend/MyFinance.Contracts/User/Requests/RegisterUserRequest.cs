namespace MyFinance.Contracts.User.Requests;

public sealed record RegisterUserRequest(
    string Name,
    string Email,
    string PlainTextPassword,
    string PlainTextPasswordConfirmation)
{
}