using MyFinance.Domain.Entities;

namespace MyFinance.Domain.Interfaces
{
    public interface IEntityRepository<TEntity> where TEntity : Entity
    {
        Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken);
        Task<TEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<bool> ExistsByIdAsync(Guid id, CancellationToken cancellationToken);
        TEntity Insert(TEntity entity);
        TEntity Update(TEntity entity);
        void DeleteById(Guid id);
    }
}
