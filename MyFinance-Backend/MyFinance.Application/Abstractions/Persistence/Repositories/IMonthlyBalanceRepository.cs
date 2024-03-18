using MyFinance.Domain.Entities;

namespace MyFinance.Application.Abstractions.Persistence.Repositories;

public interface IMonthlyBalanceRepository : IUserOwnedEntityRepository<MonthlyBalance>
{
    Task<MonthlyBalance?> GetWithSummaryData(
        Guid id,
        Guid userId,
        CancellationToken cancellationToken);

    Task<MonthlyBalance?> GetByReferenceDateAndBusinessUnitId(
        DateTime referenceDate,
        Guid businessUnitId,
        Guid userId,
        CancellationToken cancellationToken);

    Task<IEnumerable<MonthlyBalance>> GetPaginatedByBusinessUnitIdAsync(
        Guid businessUnitId,
        int pageNumber,
        int pageSize,
        Guid userId,
        CancellationToken cancellationToken);
}