using MyFinance.Application.Common.RequestHandling.Queries;
using MyFinance.Domain.Entities;

namespace MyFinance.Application.UseCases.MonthlyBalances.Queries.GetMonthlyBalances;

public sealed class GetMonthlyBalancesQuery : IQuery<IEnumerable<MonthlyBalance>>
{
    public Guid BusinessUnitId { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }

    public GetMonthlyBalancesQuery(Guid businessUnitId, int page, int pageSize)
        => (BusinessUnitId, Page, PageSize) = (businessUnitId, page, pageSize);
}
