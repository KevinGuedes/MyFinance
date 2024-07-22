using MyFinance.Application.Abstractions.RequestHandling;
using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Contracts.ManagementUnit.Responses;

namespace MyFinance.Application.UseCases.ManagementUnits.Commands.CreateManagementUnit;

public sealed record CreateManagementUnitCommand(string Name, string? Description)
    : IUserRequiredRequest, ICommand<ManagementUnitResponse>
{
    public Guid CurrentUserId { get; set; }
}