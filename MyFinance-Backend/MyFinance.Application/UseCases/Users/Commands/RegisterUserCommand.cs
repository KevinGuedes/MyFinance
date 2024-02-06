using MyFinance.Application.Common.RequestHandling.Commands;
using MyFinance.Domain.Entities;

namespace MyFinance.Application.UseCases.Users.Commands;

public sealed record RegisterUserCommand(
    string Name,
    string Email,
    string PlainTextPassword) : ICommand<User>
{
}
