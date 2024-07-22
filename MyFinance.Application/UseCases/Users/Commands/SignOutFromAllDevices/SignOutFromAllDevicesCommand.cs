using MyFinance.Application.Abstractions.RequestHandling;
using MyFinance.Application.Abstractions.RequestHandling.Commands;

namespace MyFinance.Application.UseCases.Users.Commands.SignOutFromAllDevices;

public sealed record SignOutFromAllDevicesCommand : ICommand, IUserRequiredRequest
{
    public Guid CurrentUserId { get; set; }
}
