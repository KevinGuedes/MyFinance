using MyFinance.Domain.Entities;

namespace MyFinance.Application.Abstractions.Persistence.Repositories;

public interface IAccountTagRepository
{
    Task<long> GetTotalCountAsync(Guid managementUnitId, CancellationToken cancellationToken);
    Task<IEnumerable<AccountTag>> GetPaginatedAsync(
        Guid managementUnitId,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken);
    Task<bool> ExistsByTagAsync(string tag, CancellationToken cancellationToken);
    Task<AccountTag?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<AccountTag?> GetByTagAsync(string tag, CancellationToken cancellationToken);
    Task InsertAsync(AccountTag accountTag, CancellationToken cancellationToken);
    void Update(AccountTag accountTag);
}