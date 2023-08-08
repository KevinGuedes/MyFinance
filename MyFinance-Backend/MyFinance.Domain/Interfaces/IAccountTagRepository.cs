using MyFinance.Domain.Entities;

namespace MyFinance.Domain.Interfaces;

public interface IAccountTagRepository : IEntityRepository<AccountTag>
{
    Task<bool> ExistsByTagAsync(string tag, CancellationToken cancellationToken);
}
