using MyFinance.Domain.Entities;

namespace MyFinance.Domain.Interfaces;

public interface IEntityRepository<TEntity> where TEntity : Entity
{
    Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    void Insert(TEntity entity);
    void Update(TEntity entity);
    void Delete(TEntity entity);
}
