using MyFinance.Domain.Entities;
using MyFinance.Domain.Interfaces;
using MyFinance.Infra.Data.Context;

namespace MyFinance.Infra.Data.Repositories
{
    public abstract class EntityRepository<TEntity> : IEntityRepository<TEntity> where TEntity : Entity
    {
        protected readonly MyFinanceDbContext _myFinanceDbContext;

        protected EntityRepository(MyFinanceDbContext myFinanceDbContext)
            => _myFinanceDbContext = myFinanceDbContext;

        public void DeleteById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public TEntity Insert(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public TEntity Update(TEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
