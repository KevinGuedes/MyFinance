using MyFinance.Application.Abstractions.RequestHandling.Queries;
using MyFinance.Contracts.Transfer.Responses;

namespace MyFinance.Application.UseCases.Transfers.Queries.GetBalanceDataFromPeriod;

public sealed record GetBalanceDataFromPeriodQuery(
    Guid ManagementUnitId,
    DateTime? StartDate,
    DateTime? EndDate,
    Guid? CategoryId,
    Guid? AccountTagId) : IQuery<PeriodBalanceDataResponse>
{
}
