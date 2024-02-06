using MyFinance.Application.Common.RequestHandling.Commands;
using MyFinance.Domain.Entities;

namespace MyFinance.Application.UseCases.BusinessUnits.Commands.UpdateBusinessUnit;

public sealed record UpdateBusinessUnitCommand(
    Guid Id,
    string Name,
    string Description) : ICommand<BusinessUnit>
{
}
