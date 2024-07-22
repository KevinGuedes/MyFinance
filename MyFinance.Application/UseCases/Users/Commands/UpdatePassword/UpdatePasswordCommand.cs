using MyFinance.Application.Abstractions.RequestHandling;
using MyFinance.Application.Abstractions.RequestHandling.Commands;

namespace MyFinance.Application.UseCases.Users.Commands.UpdatePassword;

public sealed record UpdatePasswordCommand(
    string PlainTextCurrentPassword,
    string PlainTextNewPassword,
    string PlainTextNewPasswordConfirmation)
    : IUserRequiredRequest, ICommand
{
    public Guid CurrentUserId { get; set; }
}
