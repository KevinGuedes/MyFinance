using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Contracts.User.Requests;

namespace MyFinance.Application.UseCases.Users.Commands.SendMagicSignInEmail;

public sealed class SendMagicSignInEmailCommand(SendMagicSignInEmailRequest request) : ICommand
{
    public string Email { get; init; } = request.Email;
}
