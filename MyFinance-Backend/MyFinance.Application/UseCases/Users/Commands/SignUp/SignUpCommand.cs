using MyFinance.Application.Abstractions.RequestHandling.Commands;

namespace MyFinance.Application.UseCases.Users.Commands.SignUp;

public sealed record SignUpCommand(
    string Name,
    string Email,
    string PlainTextPassword,
    string PlainTextPasswordConfirmation) : ICommand
{
}