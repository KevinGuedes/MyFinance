using MyFinance.Application.Abstractions.RequestHandling;
using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Contracts.ManagementUnit.Requests;
using MyFinance.Contracts.ManagementUnit.Responses;

namespace MyFinance.Application.UseCases.ManagementUnits.Commands.CreateManagementUnit;

public sealed class CreateManagementUnitCommand(CreateManagementUnitRequest request)
    : IUserRequiredRequest, ICommand<ManagementUnitResponse>
{
    public Guid CurrentUserId { get; set; }
    public string Name { get; init; } = request.Name;
    public string? Description { get; init; } = request.Description;
}