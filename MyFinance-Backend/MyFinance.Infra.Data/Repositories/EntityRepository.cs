using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MyFinance.Domain.Entities;
using MyFinance.Domain.Interfaces;
using MyFinance.Infra.Data.Context;

namespace MyFinance.Infra.Data.Repositories
{
    public abstract class EntityRepository<TEntity> : IEntityRepository<TEntity> 
        where TEntity : Entity, IAggregateRoot
    {
        private protected readonly IMongoContext _mongoContext;
        private protected readonly IMongoCollection<TEntity> _collection;

        protected EntityRepository(IMongoContext mongoContext)
            => (_mongoContext, _collection) = (mongoContext, mongoContext.GetCollection<TEntity>());

        public Task<bool> ExistsByIdAsync(Guid id, CancellationToken cancellationToken)
             => _collection.AsQueryable()
                .AnyAsync(entity => entity.Id == id, cancellationToken);

        public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken)
            => await _collection.AsQueryable()
                .ToListAsync(cancellationToken);

        public Task<TEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken)
            => _collection.AsQueryable()
                .FirstOrDefaultAsync(entity => entity.Id == id, cancellationToken);

        public TEntity Insert(TEntity entity)
        {
            _mongoContext.AddCommand((session, cancellationToken) =>
            {
                return _collection.InsertOneAsync(
                    session, 
                    entity,
                    cancellationToken: cancellationToken);
            });

            return entity;
        }

        public TEntity Update(TEntity updatedEntity)
        {
            _mongoContext.AddCommand((session, cancellationToken) =>
            {
                return _collection.ReplaceOneAsync(
                    session, 
                    entity => entity.Id == updatedEntity.Id, 
                    updatedEntity, 
                    new ReplaceOptions { IsUpsert = false }, 
                    cancellationToken);
            });

            return updatedEntity;
        }

        public void DeleteById(Guid id)
        {
            _mongoContext.AddCommand((session, cancellationToken) =>
            {
                return _collection.DeleteOneAsync(
                    session, 
                    entity => entity.Id == id,
                    cancellationToken: cancellationToken);
            });
        }
    }
}
