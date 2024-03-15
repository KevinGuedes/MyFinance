using MyFinance.Domain.Entities;

namespace MyFinance.Application.Abstractions.Persistence.Repositories;

public interface IAccountTagRepository : IUserOwnedEntityRepository<AccountTag>
{
    Task<IEnumerable<AccountTag>> GetPaginatedAsync(int page, int pageSize, Guid userId,
        CancellationToken cancellationToken);

    Task<bool> ExistsByTagAsync(string tag, Guid userId, CancellationToken cancellationToken);
    Task<AccountTag?> GetByTagAsync(string tag, Guid userId, CancellationToken cancellationToken);
}