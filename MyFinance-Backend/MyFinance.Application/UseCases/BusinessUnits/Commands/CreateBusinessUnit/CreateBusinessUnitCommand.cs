using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Application.Common.RequestHandling;
using MyFinance.Contracts.BusinessUnit.Responses;

namespace MyFinance.Application.UseCases.BusinessUnits.Commands.CreateBusinessUnit;

public sealed record CreateBusinessUnitCommand(string Name, string? Description) 
    : UserBasedRequest, ICommand<BusinessUnitResponse>
{
}