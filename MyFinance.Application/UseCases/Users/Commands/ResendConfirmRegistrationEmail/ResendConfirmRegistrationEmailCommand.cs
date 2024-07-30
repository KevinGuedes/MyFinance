using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Contracts.User.Requests;

namespace MyFinance.Application.UseCases.Users.Commands.ResendConfirmRegistrationEmail;

public sealed class ResendConfirmRegistrationEmailCommand(ResendConfirmRegistrationEmailRequest request)
    : ICommand
{
    public string Email { get; init; } = request.Email;
}
