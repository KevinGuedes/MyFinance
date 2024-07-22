using MyFinance.Application.Abstractions.RequestHandling.Commands;

namespace MyFinance.Application.UseCases.Users.Commands.ResendConfirmRegistrationEmail;

public sealed record ResendConfirmRegistrationEmailCommand(string Email) : ICommand
{
}
