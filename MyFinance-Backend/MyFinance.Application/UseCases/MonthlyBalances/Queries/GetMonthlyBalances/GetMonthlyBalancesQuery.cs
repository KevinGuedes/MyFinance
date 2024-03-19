using MyFinance.Application.Abstractions.RequestHandling.Queries;
using MyFinance.Application.Common.RequestHandling;
using MyFinance.Contracts.Common;
using MyFinance.Contracts.MonthlyBalance.Responses;

namespace MyFinance.Application.UseCases.MonthlyBalances.Queries.GetMonthlyBalances;

public sealed record GetMonthlyBalancesQuery(Guid BusinessUnitId, int PageNumber, int PageSize)
    : UserBasedRequest, IQuery<Paginated<MonthlyBalanceResponse>>
{
}