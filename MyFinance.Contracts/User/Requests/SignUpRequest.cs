namespace MyFinance.Contracts.User.Requests;

public sealed record SignUpRequest(
    string Name,
    string Email,
    string PlainTextPassword,
    string PlainTextPasswordConfirmation)
{
}