using MyFinance.Domain.Entities;

namespace MyFinance.Domain.Interfaces
{
    public interface IEntityRepository<TEntity> where TEntity : Entity
    {
        Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken);
        Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<bool> ExistsByIdAsync(Guid id, CancellationToken cancellationToken);
        void Insert(TEntity entity);
        void Update(TEntity entity);
        void DeleteByIdAsync(TEntity entity);
    }
}
