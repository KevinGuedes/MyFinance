using MyFinance.Application.Generics.Requests;
using MyFinance.Domain.Entities;

namespace MyFinance.Application.MonthlyBalances.Queries.GetRecentMonthlyBalances
{
    public sealed class GetMonthlyBalancesQuery : Query<IEnumerable<MonthlyBalance>>
    {
        public int Count { get; set; }
        public int Skip { get; set; }

        public GetMonthlyBalancesQuery(int count, int skip)
            => (Count, Skip) = (count, skip);

        public GetMonthlyBalancesQuery()
          => (Count, Skip) = (3, 0);
    }
}
