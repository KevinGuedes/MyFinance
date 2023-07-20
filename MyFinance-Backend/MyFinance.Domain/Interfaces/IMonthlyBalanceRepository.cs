using MyFinance.Domain.Entities;

namespace MyFinance.Domain.Interfaces;

public interface IMonthlyBalanceRepository : IEntityRepository<MonthlyBalance>
{
    Task<IEnumerable<MonthlyBalance>> GetByBusinessUnitId(Guid businessUnitId, int take, int skip, CancellationToken cancellationToken);
    Task<MonthlyBalance?> GetByReferenceDateAndBusinessUnitId(DateTime referenceDate, Guid businessUnitId, CancellationToken cancellationToken);
}
