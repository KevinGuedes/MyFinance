using MyFinance.Application.Abstractions.RequestHandling.Commands;

namespace MyFinance.Application.UseCases.Users.Commands.SendMagicSignInEmail;

public sealed record SendMagicSignInEmailCommand(string Email) : ICommand
{
}
