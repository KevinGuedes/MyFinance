using Microsoft.EntityFrameworkCore;
using MyFinance.Domain.Entities;
using MyFinance.Domain.Interfaces;
using MyFinance.Infra.Data.Context;

namespace MyFinance.Infra.Data.Repositories;

public abstract class EntityRepository<TEntity> : IEntityRepository<TEntity> where TEntity : Entity
{
    protected readonly MyFinanceDbContext _myFinanceDbContext;

    protected EntityRepository(MyFinanceDbContext myFinanceDbContext)
        => _myFinanceDbContext = myFinanceDbContext;

    public Task<bool> ExistsByIdAsync(Guid id, CancellationToken cancellationToken)
        => _myFinanceDbContext.Set<TEntity>().AnyAsync(entity => entity.Id == id, cancellationToken);

    public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken)
       => await _myFinanceDbContext.Set<TEntity>().AsNoTracking().ToListAsync(cancellationToken);

    public virtual async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => await _myFinanceDbContext.Set<TEntity>().FindAsync(id, cancellationToken);

    public void Insert(TEntity entity)
        => _myFinanceDbContext.Set<TEntity>().Add(entity);

    public void Update(TEntity entity)
        => _myFinanceDbContext.Set<TEntity>().Update(entity);

    public void Delete(TEntity entity)
        => _myFinanceDbContext.Set<TEntity>().Remove(entity);
}
