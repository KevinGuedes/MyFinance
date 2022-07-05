using MyFinance.Domain.Entities;

namespace MyFinance.Domain.Interfaces
{
    public interface IMonthlyBalanceRepository : IEntityRepository<MonthlyBalance>
    {
        Task<IEnumerable<MonthlyBalance>> GetByBusinessUnitId(Guid businessUnitId, int count, int skip, CancellationToken cancellationToken);
        Task<MonthlyBalance> GetByMonthAndYearAsync(int month, int year, CancellationToken cancellationToken);
    }
}
