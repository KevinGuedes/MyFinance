using MyFinance.Domain.Entities;

namespace MyFinance.Application.Abstractions.Persistence.Repositories;

public interface IManagementUnitRepository
{
    Task<long> GetTotalCountAsync(CancellationToken cancellationToken);
    Task<IEnumerable<ManagementUnit>> GetPaginatedAsync(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken);
    Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken);
    Task<ManagementUnit?> GetByNameAsync(string name, CancellationToken cancellationToken);
    Task<ManagementUnit?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task InsertAsync(ManagementUnit managementUnit, CancellationToken cancellationToken);
    void Update(ManagementUnit managementUnit);
}