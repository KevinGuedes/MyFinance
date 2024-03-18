using MyFinance.Application.Abstractions.RequestHandling.Queries;
using MyFinance.Contracts.Common;
using MyFinance.Contracts.MonthlyBalance.Responses;

namespace MyFinance.Application.UseCases.MonthlyBalances.Queries.GetMonthlyBalances;

public sealed record GetMonthlyBalancesQuery(Guid BusinessUnitId, int PageNumber, int PageSize)
    : IQuery<PaginatedResponse<MonthlyBalanceResponse>>
{
}