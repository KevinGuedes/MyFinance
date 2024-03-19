using MyFinance.Application.Abstractions.RequestHandling;
using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Contracts.BusinessUnit.Responses;

namespace MyFinance.Application.UseCases.BusinessUnits.Commands.CreateBusinessUnit;

public sealed record CreateBusinessUnitCommand(string Name, string? Description) 
    : ICommand<BusinessUnitResponse>, IUserBasedRequest
{
    public Guid CurrentUserId { get; set; }
}