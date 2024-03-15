using MyFinance.Application.Abstractions.RequestHandling.Commands;

namespace MyFinance.Application.UseCases.BusinessUnits.Commands.UnarchiveBusinessUnit;

public sealed record UnarchiveBusinessUnitCommand(Guid Id) : ICommand
{
}
