using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Contracts.User.Requests;

namespace MyFinance.Application.UseCases.Users.Commands.SendResetPasswordEmail;

public sealed class SendResetPasswordEmailCommand(SendResetPasswordEmailRequest request) : ICommand
{
    public string Email { get; init; } = request.Email;
}
