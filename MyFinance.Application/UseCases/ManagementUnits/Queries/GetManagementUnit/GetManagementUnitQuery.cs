using MyFinance.Application.Abstractions.RequestHandling.Queries;
using MyFinance.Contracts.ManagementUnit.Responses;

namespace MyFinance.Application.UseCases.ManagementUnits.Queries.GetManagementUnit;

public sealed record GetManagementUnitQuery(Guid Id) : IQuery<ManagementUnitResponse>
{
}