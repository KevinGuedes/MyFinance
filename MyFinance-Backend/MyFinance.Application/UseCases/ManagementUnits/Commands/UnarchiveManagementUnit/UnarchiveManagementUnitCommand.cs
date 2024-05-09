using MyFinance.Application.Abstractions.RequestHandling.Commands;

namespace MyFinance.Application.UseCases.ManagementUnits.Commands.UnarchiveManagementUnit;

public sealed record UnarchiveManagementUnitCommand(Guid Id) : ICommand
{
}