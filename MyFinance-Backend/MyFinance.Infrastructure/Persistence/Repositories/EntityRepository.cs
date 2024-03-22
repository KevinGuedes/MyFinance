using Microsoft.EntityFrameworkCore;
using MyFinance.Domain.Common;
using MyFinance.Infrastructure.Persistence.Context;

namespace MyFinance.Infrastructure.Persistence.Repositories;

internal abstract class EntityRepository<TEntity>(MyFinanceDbContext myFinanceDbContext)
    where TEntity : Entity
{
    protected readonly MyFinanceDbContext _myFinanceDbContext = myFinanceDbContext;

    public Task<int> GetTotalCountAsync(CancellationToken cancellationToken)
        => _myFinanceDbContext.Set<TEntity>().CountAsync(cancellationToken);

    public virtual async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => await _myFinanceDbContext.Set<TEntity>().FindAsync([id], cancellationToken);

    public Task<bool> ExistsByIdAsync(Guid id, CancellationToken cancellationToken)
        => _myFinanceDbContext.Set<TEntity>().AnyAsync(entity => entity.Id == id, cancellationToken);

    public void Insert(TEntity entity)
        => _myFinanceDbContext.Set<TEntity>().Add(entity);

    public void Update(TEntity entity)
        => _myFinanceDbContext.Set<TEntity>().Update(entity);

    public void Delete(TEntity entity)
        => _myFinanceDbContext.Set<TEntity>().Remove(entity);
}