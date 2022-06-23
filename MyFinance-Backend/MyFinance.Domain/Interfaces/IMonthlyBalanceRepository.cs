using MyFinance.Domain.Entities;

namespace MyFinance.Domain.Interfaces
{
    public interface IMonthlyBalanceRepository : IEntityRepository<MonthlyBalance>
    {
        Task<IEnumerable<MonthlyBalance>> GetMonthlyBalances(int count, int skip, CancellationToken cancellationToken);
    }
}
