using MyFinance.Domain.Entities;

namespace MyFinance.Application.Abstractions.Persistence.Repositories;

public interface IBusinessUnitRepository : IUserOwnedEntityRepository<BusinessUnit>
{
    Task<IEnumerable<BusinessUnit>> GetPaginatedAsync(int page, int pageSize, Guid userId,
        CancellationToken cancellationToken);

    Task<bool> ExistsByNameAsync(string name, Guid userId, CancellationToken cancellationToken);
    Task<BusinessUnit?> GetByNameAsync(string name, Guid userId, CancellationToken cancellationToken);
    Task<BusinessUnit?> GetWithSummaryData(Guid id, int year, Guid userId, CancellationToken cancellationToken);
}