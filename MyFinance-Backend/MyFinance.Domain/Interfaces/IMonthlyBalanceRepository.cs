using MyFinance.Domain.Entities;

namespace MyFinance.Domain.Interfaces;

public interface IMonthlyBalanceRepository : IEntityRepository<MonthlyBalance>
{
    Task<MonthlyBalance?> GetByReferenceDateAndBusinessUnitId(
        DateTime referenceDate,
        Guid businessUnitId,
        CancellationToken cancellationToken);
    Task<IEnumerable<MonthlyBalance>> GetPaginatedByBusinessUnitIdAsync(
        Guid businessUnitId,
        int page,
        int pageSize,
        CancellationToken cancellationToken);
}
