using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Contracts.User.Requests;

namespace MyFinance.Application.UseCases.Users.Commands.SignUp;

public sealed class SignUpCommand(SignUpRequest request) : ICommand
{
    public string Name { get; init; } = request.Name;
    public string Email { get; init; } = request.Email;
    public string PlainTextPassword { get; init; } = request.PlainTextPassword;
    public string PlainTextPasswordConfirmation { get; init; } = request.PlainTextPasswordConfirmation;
}