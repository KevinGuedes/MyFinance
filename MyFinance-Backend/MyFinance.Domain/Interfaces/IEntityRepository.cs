using MyFinance.Domain.Entities;

namespace MyFinance.Domain.Interfaces;

public interface IEntityRepository<TEntity> where TEntity : Entity
{
    Task<IReadOnlyCollection<TEntity>> GetAllAsync(CancellationToken cancellationToken);
    Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<bool> ExistsByIdAsync(Guid id, CancellationToken cancellationToken);
    void Insert(TEntity entity);
    void Update(TEntity entity);
    void Delete(TEntity entity);
}
