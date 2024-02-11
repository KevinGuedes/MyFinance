using MyFinance.Domain.Entities;

namespace MyFinance.Domain.Interfaces;

public interface IUserRepository : IEntityRepository<User>
{
    Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken);
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);
}
