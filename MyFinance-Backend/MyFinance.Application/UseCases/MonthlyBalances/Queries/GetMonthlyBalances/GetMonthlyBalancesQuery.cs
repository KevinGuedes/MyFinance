using MyFinance.Application.Abstractions.RequestHandling.Queries;
using MyFinance.Domain.Entities;

namespace MyFinance.Application.UseCases.MonthlyBalances.Queries.GetMonthlyBalances;

public sealed record GetMonthlyBalancesQuery(Guid BusinessUnitId, int Page, int PageSize)
    : IQuery<IEnumerable<MonthlyBalance>>
{
}