namespace MyFinance.Contracts.User.Requests;

public sealed record SignInRequest(string Email, string PlainTextPassword)
{
}