using MyFinance.Domain.Entities;

namespace MyFinance.Application.Abstractions.Persistence.Repositories;

public interface IUserOwnedEntityRepository<TUserOwnedEntity> where TUserOwnedEntity : UserOwnedEntity
{
    Task<TUserOwnedEntity?> GetByIdAsync(Guid id, Guid userId, CancellationToken cancellationToken);
    Task<bool> ExistsByIdAsync(Guid id, Guid userId, CancellationToken cancellationToken);
    void Insert(TUserOwnedEntity entity);
    void Update(TUserOwnedEntity entity);
    void Delete(TUserOwnedEntity entity);
}