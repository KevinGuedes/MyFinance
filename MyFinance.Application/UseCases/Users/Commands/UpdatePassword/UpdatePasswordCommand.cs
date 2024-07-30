using MyFinance.Application.Abstractions.RequestHandling;
using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Contracts.User.Requests;

namespace MyFinance.Application.UseCases.Users.Commands.UpdatePassword;

public sealed class UpdatePasswordCommand(UpdatePasswordRequest request)
    : IUserRequiredRequest, ICommand
{
    public Guid CurrentUserId { get; set; }
    public string PlainTextCurrentPassword { get; init; } = request.PlainTextCurrentPassword;
    public string PlainTextNewPassword { get; init; } = request.PlainTextNewPassword;
    public string PlainTextNewPasswordConfirmation { get; init; } = request.PlainTextNewPasswordConfirmation;
}
