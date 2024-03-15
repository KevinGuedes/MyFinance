using Microsoft.EntityFrameworkCore;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Domain.Entities;
using MyFinance.Infrastructure.Persistence.Context;

namespace MyFinance.Infrastructure.Persistence.Repositories;

public abstract class EntityRepository<TEntity>(MyFinanceDbContext myFinanceDbContext) 
    : IEntityRepository<TEntity>
    where TEntity : Entity
{
    protected readonly MyFinanceDbContext _myFinanceDbContext = myFinanceDbContext;

    public virtual async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => await _myFinanceDbContext.Set<TEntity>().FindAsync(new object[] { id }, cancellationToken);

    public Task<bool> ExistsByIdAsync(Guid id, CancellationToken cancellationToken)
        => _myFinanceDbContext.Set<TEntity>().AnyAsync(entity => entity.Id == id, cancellationToken);

    public void Insert(TEntity entity)
        => _myFinanceDbContext.Set<TEntity>().Add(entity);

    public void Update(TEntity entity)
        => _myFinanceDbContext.Set<TEntity>().Update(entity);

    public void Delete(TEntity entity)
        => _myFinanceDbContext.Set<TEntity>().Remove(entity);
}
