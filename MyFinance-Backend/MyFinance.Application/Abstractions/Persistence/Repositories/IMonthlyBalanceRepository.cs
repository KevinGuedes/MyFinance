using MyFinance.Domain.Entities;

namespace MyFinance.Application.Abstractions.Persistence.Repositories;

public interface IMonthlyBalanceRepository
{
    Task<MonthlyBalance?> GetWithSummaryData(Guid id, CancellationToken cancellationToken);
    
    Task<MonthlyBalance?> GetByReferenceDateAndBusinessUnitId(
        DateTime referenceDate,
        Guid businessUnitId,
        CancellationToken cancellationToken);

    Task<IEnumerable<MonthlyBalance>> GetPaginatedByBusinessUnitIdAsync(
        Guid businessUnitId,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken);

    void Update(MonthlyBalance monthlyBalance);
    void Insert(MonthlyBalance monthlyBalance);
}