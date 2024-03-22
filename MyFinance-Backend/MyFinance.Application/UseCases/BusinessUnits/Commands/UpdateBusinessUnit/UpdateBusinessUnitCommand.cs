using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Contracts.BusinessUnit.Responses;

namespace MyFinance.Application.UseCases.BusinessUnits.Commands.UpdateBusinessUnit;

public sealed record UpdateBusinessUnitCommand(Guid Id, string Name, string? Description) 
    : ICommand<BusinessUnitResponse>
{
}