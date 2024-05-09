using MyFinance.Application.Abstractions.RequestHandling.Queries;
using MyFinance.Contracts.Common;
using MyFinance.Contracts.ManagementUnit.Responses;

namespace MyFinance.Application.UseCases.ManagementUnits.Queries.GetManagementUnits;

public sealed record GetManagementUnitsQuery(int PageNumber, int PageSize) 
    : IQuery<Paginated<ManagementUnitResponse>>
{
}