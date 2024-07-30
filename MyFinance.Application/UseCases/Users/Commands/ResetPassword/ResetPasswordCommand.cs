using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Contracts.User.Requests;

namespace MyFinance.Application.UseCases.Users.Commands.ResetPassword;

public sealed class ResetPasswordCommand(ResetPasswordRequest request)
    : ICommand
{
    public string UrlSafeResetPasswordToken { get; init; } = request.UrlSafeResetPasswordToken;
    public string PlainTextNewPassword { get; init; } = request.PlainTextNewPassword;
    public string PlainTextNewPasswordConfirmation { get; init; } = request.PlainTextNewPasswordConfirmation;
}
