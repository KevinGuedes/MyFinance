using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Contracts.ManagementUnit.Responses;

namespace MyFinance.Application.UseCases.ManagementUnits.Commands.UpdateManagementUnit;

public sealed record UpdateManagementUnitCommand(Guid Id, string Name, string? Description)
    : ICommand<ManagementUnitResponse>
{
}