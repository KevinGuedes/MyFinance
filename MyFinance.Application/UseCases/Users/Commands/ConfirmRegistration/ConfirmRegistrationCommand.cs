using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Contracts.User.Requests;

namespace MyFinance.Application.UseCases.Users.Commands.ConfirmRegistration;

public sealed class ConfirmRegistrationCommand(ConfirmRegistrationRequest request)
    : ICommand
{
    public string UrlSafeConfirmRegistrationToken { get; init; } = request.UrlSafeConfirmRegistrationToken;
}
