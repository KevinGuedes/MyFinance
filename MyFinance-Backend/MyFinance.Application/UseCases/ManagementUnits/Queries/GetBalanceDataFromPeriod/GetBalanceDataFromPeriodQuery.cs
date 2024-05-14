using MyFinance.Application.Abstractions.RequestHandling.Queries;
using MyFinance.Contracts.ManagementUnit.Responses;

namespace MyFinance.Application.UseCases.ManagementUnits.Queries.GetBalanceDataFromPeriod;

public sealed record class GetBalanceDataFromPeriodQuery(
    Guid ManagementUnitId,
    DateOnly? StartDate,
    DateOnly? EndDate,
    Guid? CategoryId,
    Guid? AccountTagId) : IQuery<PeriodBalanceDataResponse>
{
}
