using MyFinance.Domain.Entities;

namespace MyFinance.Domain.Interfaces
{
    public interface IMonthlyBalanceRepository : IEntityRepository<MonthlyBalance>
    {
        Task<IEnumerable<MonthlyBalance>> GetAllAsync(int count, int skip, CancellationToken cancellationToken);
        Task<MonthlyBalance> GetByMonthAndYearAsync(int month, int year, CancellationToken cancellationToken);
        Task<bool> ExistsByMonthAndYearAsync(int month, int year, CancellationToken cancellationToken);
    }
}
