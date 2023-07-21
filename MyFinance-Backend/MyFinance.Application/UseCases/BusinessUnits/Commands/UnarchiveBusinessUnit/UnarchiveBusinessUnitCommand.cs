using MyFinance.Application.Common.RequestHandling.Commands;

namespace MyFinance.Application.UseCases.BusinessUnits.Commands.UnarchiveBusinessUnit;

public class UnarchiveBusinessUnitCommand : ICommand
{
    public Guid Id { get; set; }

    public UnarchiveBusinessUnitCommand(Guid id)
        => Id = id;
}
