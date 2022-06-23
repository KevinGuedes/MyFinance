using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MyFinance.Domain.Entities;
using MyFinance.Domain.Interfaces;
using MyFinance.Infra.Data.Context;
using MyFinance.Infra.Data.UnitOfWork;

namespace MyFinance.Infra.Data.Repositories
{
    public abstract class EntityRepository<TEntity> : IEntityRepository<TEntity> where TEntity : Entity
    {
        private protected readonly IMongoContext _mongoContext;
        private protected readonly IMongoCollection<TEntity> _collection;

        protected EntityRepository(IMongoContext mongoContext, IUnitOfWork unitOfWork)
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
                return _collection.InsertOneAsync(session, entity, cancellationToken: cancellationToken);
            });

            return entity;
        }

        public TEntity Update(TEntity updatedEntity)
        {
            _mongoContext.AddCommand((session, cancellationToken) =>
            {
                var filter = Builders<TEntity>.Filter.Eq(entity => entity.Id, updatedEntity.Id);
                return _collection.ReplaceOneAsync(session, filter, updatedEntity, cancellationToken: cancellationToken);
            });

            return updatedEntity;
        }

        public void RemoveById(Guid id)
        {
            _mongoContext.AddCommand((session, cancellationToken) =>
            {
                var filter = Builders<TEntity>.Filter.Eq(entity => entity.Id, id);
                return _collection.DeleteOneAsync(session, filter, cancellationToken: cancellationToken);
            });
        }
    }
}
