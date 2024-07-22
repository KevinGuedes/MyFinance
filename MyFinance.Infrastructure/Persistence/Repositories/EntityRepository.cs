using Microsoft.EntityFrameworkCore;
using MyFinance.Domain.Common;
using MyFinance.Infrastructure.Persistence.Context;

namespace MyFinance.Infrastructure.Persistence.Repositories;

internal abstract class EntityRepository<TEntity>(MyFinanceDbContext myFinanceDbContext)
    where TEntity : Entity
{
    protected readonly MyFinanceDbContext _myFinanceDbContext = myFinanceDbContext;

    public Task<long> GetTotalCountAsync(CancellationToken cancellationToken)
        => _myFinanceDbContext.Set<TEntity>().LongCountAsync(cancellationToken);

    public virtual async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => await _myFinanceDbContext.Set<TEntity>().FindAsync([id], cancellationToken);

    public Task<bool> ExistsByIdAsync(Guid id, CancellationToken cancellationToken)
        => _myFinanceDbContext.Set<TEntity>().AnyAsync(entity => entity.Id == id, cancellationToken);

    public async Task InsertAsync(TEntity entity, CancellationToken cancellationToken)
        => await _myFinanceDbContext.Set<TEntity>().AddAsync(entity, cancellationToken);

    public void Update(TEntity entity)
        => _myFinanceDbContext.Set<TEntity>().Update(entity);

    public void Delete(TEntity entity)
        => _myFinanceDbContext.Set<TEntity>().Remove(entity);
}