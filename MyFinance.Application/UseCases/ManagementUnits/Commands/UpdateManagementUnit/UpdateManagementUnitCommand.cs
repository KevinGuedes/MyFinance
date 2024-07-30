using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Contracts.ManagementUnit.Requests;
using MyFinance.Contracts.ManagementUnit.Responses;

namespace MyFinance.Application.UseCases.ManagementUnits.Commands.UpdateManagementUnit;

public sealed class UpdateManagementUnitCommand(UpdateManagementUnitRequest request)
    : ICommand<ManagementUnitResponse>
{
    public Guid Id { get; init; } = request.Id;
    public string Name { get; init; } = request.Name;
    public string? Description { get; init; } = request.Description;
}