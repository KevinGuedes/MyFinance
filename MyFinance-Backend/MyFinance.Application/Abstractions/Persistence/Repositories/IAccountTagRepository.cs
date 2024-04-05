using MyFinance.Domain.Entities;

namespace MyFinance.Application.Abstractions.Persistence.Repositories;

public interface IAccountTagRepository
{
    Task<long> GetTotalCountAsync(CancellationToken cancellationToken);
    Task<IEnumerable<AccountTag>> GetPaginatedAsync(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken);

    Task<bool> ExistsByTagAsync(string tag, CancellationToken cancellationToken);
    Task<AccountTag?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<AccountTag?> GetByTagAsync(string tag, CancellationToken cancellationToken);
    void Update(AccountTag accountTag);
    void Insert(AccountTag accountTag);
}