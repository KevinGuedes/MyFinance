using MyFinance.Application.Common.RequestHandling.Commands;

namespace MyFinance.Application.UseCases.Users.Commands.RegisterUser;

public sealed record RegisterUserCommand(
    string Name,
    string Email,
    string PlainTextPassword) : ICommand
{
}
