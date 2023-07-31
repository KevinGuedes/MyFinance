using MyFinance.Application.Common.RequestHandling.Commands;

namespace MyFinance.Application.UseCases.BusinessUnits.Commands.UnarchiveBusinessUnit;

public sealed record UnarchiveBusinessUnitCommand(Guid Id) : ICommand
{
}
