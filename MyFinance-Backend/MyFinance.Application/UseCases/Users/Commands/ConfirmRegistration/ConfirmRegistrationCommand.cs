using MyFinance.Application.Abstractions.RequestHandling.Commands;

namespace MyFinance.Application.UseCases.Users.Commands.ConfirmRegistration;

public sealed record ConfirmRegistrationCommand(string UrlSafeConfirmRegistrationToken) 
    : ICommand
{
}
