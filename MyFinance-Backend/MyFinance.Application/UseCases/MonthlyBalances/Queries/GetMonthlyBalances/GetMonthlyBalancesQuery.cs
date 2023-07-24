using MyFinance.Application.Common.RequestHandling.Queries;
using MyFinance.Domain.Entities;

namespace MyFinance.Application.UseCases.MonthlyBalances.Queries.GetMonthlyBalances;

public sealed class GetMonthlyBalancesQuery : IQuery<IReadOnlyCollection<MonthlyBalance>>
{
    public Guid BusinessUnitId { get; set; }
    public int Take { get; set; }
    public int Skip { get; set; }

    public GetMonthlyBalancesQuery(Guid businessUnitId, int take, int skip)
        => (BusinessUnitId, Take, Skip) = (businessUnitId, take, skip);
}
