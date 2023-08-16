using MyFinance.Domain.Entities;

namespace MyFinance.Domain.Interfaces;

public interface IBusinessUnitRepository : IEntityRepository<BusinessUnit>
{
    Task<IEnumerable<BusinessUnit>> GetPaginatedAsync(int page, int pageSize, CancellationToken cancellationToken);
    Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken);
    Task<BusinessUnit?> GetByNameAsync(string name, CancellationToken cancellationToken);
    Task<BusinessUnit?> GetWithSummaryData(Guid id, int year, CancellationToken cancellationToken);
}
