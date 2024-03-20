using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Application.Common.RequestHandling;

namespace MyFinance.Application.UseCases.BusinessUnits.Commands.UnarchiveBusinessUnit;

public sealed record UnarchiveBusinessUnitCommand(Guid Id) : UserBasedRequest, ICommand
{
}