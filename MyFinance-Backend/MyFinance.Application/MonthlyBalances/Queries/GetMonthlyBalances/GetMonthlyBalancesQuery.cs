using MediatR;
using MyFinance.Application.Interfaces;
using MyFinance.Domain.Entities;

namespace MyFinance.Application.MonthlyBalances.Queries.GetRecentMonthlyBalances
{
    public sealed class GetMonthlyBalancesQuery : IRequest<IEnumerable<MonthlyBalance>>, IQuery
    {
        public int Count { get; set; }
        public int Skip { get; set; }

        public GetMonthlyBalancesQuery(int count, int skip)
            => (Count, Skip) = (count, skip);
    }
}
