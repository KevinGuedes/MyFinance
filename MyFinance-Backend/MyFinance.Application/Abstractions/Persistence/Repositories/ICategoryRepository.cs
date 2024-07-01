using MyFinance.Domain.Entities;

namespace MyFinance.Application.Abstractions.Persistence.Repositories;

public interface ICategoryRepository
{
    Task<long> GetTotalCountAsync(Guid managementUnitId, CancellationToken cancellationToken);
    Task<IEnumerable<Category>> GetPaginatedAsync(
        Guid managementUnitId,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken);
    Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken);
    Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Category?> GetByNameAsync(string name, CancellationToken cancellationToken);
    Task InsertAsync(Category category, CancellationToken cancellationToken);
    void Update(Category category);
}
