using MyFinance.Domain.Entities;

namespace MyFinance.Domain.Interfaces;

public interface IAccountTagRepository : IUserOwnedEntityRepository<AccountTag>
{
    Task<IEnumerable<AccountTag>> GetPaginatedAsync(int page, int pageSize, Guid userId, CancellationToken cancellationToken);
    Task<bool> ExistsByTagAsync(string tag, Guid userId, CancellationToken cancellationToken);
    Task<AccountTag?> GetByTagAsync(string tag, Guid userId, CancellationToken cancellationToken);
}
