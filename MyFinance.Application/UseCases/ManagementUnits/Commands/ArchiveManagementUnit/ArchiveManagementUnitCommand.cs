using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Contracts.ManagementUnit.Requests;

namespace MyFinance.Application.UseCases.ManagementUnits.Commands.ArchiveManagementUnit;

public sealed class ArchiveManagementUnitCommand(ArchiveManagementUnitRequest request) : ICommand
{
    public Guid Id { get; init; } = request.Id;
    public string? ReasonToArchive { get; init; } = request.ReasonToArchive;
}