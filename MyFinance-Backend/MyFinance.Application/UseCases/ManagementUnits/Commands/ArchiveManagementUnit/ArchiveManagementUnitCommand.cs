using MyFinance.Application.Abstractions.RequestHandling.Commands;

namespace MyFinance.Application.UseCases.ManagementUnits.Commands.ArchiveManagementUnit;

public sealed record ArchiveManagementUnitCommand(Guid Id, string? ReasonToArchive) : ICommand
{
}