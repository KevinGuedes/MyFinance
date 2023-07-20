using MyFinance.Application.Common.RequestHandling.Commands;

namespace MyFinance.Application.UseCases.BusinessUnits.Commands.ArchiveBusinessUnit;

public sealed class ArchiveBusinessUnitCommand : ICommand
{
    public Guid Id { get; set; }
    public string ReasonToArchive { get; set; }

    public ArchiveBusinessUnitCommand(Guid id, string reasonToArchive)
        => (Id, ReasonToArchive) = (id, reasonToArchive);
}
