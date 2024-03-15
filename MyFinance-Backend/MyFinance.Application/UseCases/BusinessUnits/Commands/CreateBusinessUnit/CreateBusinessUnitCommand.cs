using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Domain.Entities;

namespace MyFinance.Application.UseCases.BusinessUnits.Commands.CreateBusinessUnit;

public sealed record CreateBusinessUnitCommand(string Name, string? Description) : ICommand<BusinessUnit>
{
}
