using MyFinance.Domain.Entities;

namespace MyFinance.Domain.Interfaces;

public interface IAccountTagRepository : IEntityRepository<AccountTag>
{
    Task<IEnumerable<AccountTag>> GetPaginatedAsync(int page, int pageSize, CancellationToken cancellationToken);
    Task<bool> ExistsByTagAsync(string tag, CancellationToken cancellationToken);
    Task<AccountTag?> GetByTagAsync(string tag, CancellationToken cancellationToken);
}
