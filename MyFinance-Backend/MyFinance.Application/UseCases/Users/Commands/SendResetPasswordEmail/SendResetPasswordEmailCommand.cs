using MyFinance.Application.Abstractions.RequestHandling.Commands;

namespace MyFinance.Application.UseCases.Users.Commands.SendResetPasswordEmail;

public sealed record SendResetPasswordEmailCommand(string Email) : ICommand
{
}
