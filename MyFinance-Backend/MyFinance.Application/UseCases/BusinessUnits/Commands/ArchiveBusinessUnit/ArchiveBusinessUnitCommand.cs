using MyFinance.Application.Common.RequestHandling.Commands;

namespace MyFinance.Application.UseCases.BusinessUnits.Commands.ArchiveBusinessUnit;

public sealed record ArchiveBusinessUnitCommand(Guid Id, string? ReasonToArchive) : ICommand
{
}
