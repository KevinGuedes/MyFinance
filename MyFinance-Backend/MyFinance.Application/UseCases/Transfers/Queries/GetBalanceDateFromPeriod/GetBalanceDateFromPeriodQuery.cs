using MyFinance.Application.Abstractions.RequestHandling.Queries;
using MyFinance.Contracts.Transfer.Responses;

namespace MyFinance.Application.UseCases.Transfers.Queries.GetPeriodBalance;

public sealed record class GetBalanceDateFromPeriodQuery(
    Guid BusinessUnitId,
    DateOnly? StartDate,
    DateOnly? EndDate,
    Guid? CategoryId,
    Guid? AccountTagId) : IQuery<PeriodBalanceDataResponse>
{
}
