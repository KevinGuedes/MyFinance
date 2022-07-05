using MyFinance.Application.Generics.Requests;
using MyFinance.Domain.Entities;

namespace MyFinance.Application.MonthlyBalances.Queries.GetMonthlyBalances
{
    public sealed class GetMonthlyBalancesQuery : Query<IEnumerable<MonthlyBalance>>
    {
        public Guid BusinessUnitId { get; set; }
        public int Count { get; set; }
        public int Skip { get; set; }

        public GetMonthlyBalancesQuery(Guid businessUnitId, int count, int skip)
            => (BusinessUnitId, Count, Skip) = (businessUnitId, count, skip);
    }
}
