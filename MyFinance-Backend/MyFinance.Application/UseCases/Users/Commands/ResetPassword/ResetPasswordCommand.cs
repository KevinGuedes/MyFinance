using MyFinance.Application.Abstractions.RequestHandling.Commands;

namespace MyFinance.Application.UseCases.Users.Commands.ResetPassword;

public sealed record ResetPasswordCommand(
    string UrlSafeResetPasswordToken,
    string PlainTextNewPassword,
    string PlainTextNewPasswordConfirmation) 
    : ICommand
{
}
