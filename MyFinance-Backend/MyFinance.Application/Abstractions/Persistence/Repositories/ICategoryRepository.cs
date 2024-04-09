using MyFinance.Domain.Entities;

namespace MyFinance.Application.Abstractions.Persistence.Repositories;

public interface ICategoryRepository
{
    Task<long> GetTotalCountAsync(CancellationToken cancellationToken);
    Task<IEnumerable<Category>> GetPaginatedAsync(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken);
    Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken);
    Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Category?> GetByNameAsync(string name, CancellationToken cancellationToken);
    void Update(Category category);
    void Insert(Category category);
}
