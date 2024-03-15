using MyFinance.Domain.Entities;

namespace MyFinance.Application.Abstractions.Persistence.Repositories;

public interface IEntityRepository<TEntity> where TEntity : Entity
{
    Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<bool> ExistsByIdAsync(Guid id, CancellationToken cancellationToken);
    void Insert(TEntity entity);
    void Update(TEntity entity);
    void Delete(TEntity entity);
}