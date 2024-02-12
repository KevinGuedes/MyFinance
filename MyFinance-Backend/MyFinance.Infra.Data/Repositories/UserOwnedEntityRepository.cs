﻿using Microsoft.EntityFrameworkCore;
using MyFinance.Domain.Entities;
using MyFinance.Infra.Data.Context;

namespace MyFinance.Domain.Interfaces;

public abstract class UserOwnedEntityRepository<TUserOwnedEntity>(MyFinanceDbContext myFinanceDbContext)
    : IUserOwnedEntityRepository<TUserOwnedEntity>
    where TUserOwnedEntity : UserOwnedEntity
{
    protected readonly MyFinanceDbContext _myFinanceDbContext = myFinanceDbContext;

    public virtual Task<TUserOwnedEntity?> GetByIdAsync(Guid id, Guid userId, CancellationToken cancellationToken)
        => _myFinanceDbContext.Set<TUserOwnedEntity>()
            .Where(entity => entity.Id == id && entity.UserId == userId)
            .FirstOrDefaultAsync(cancellationToken);

    public Task<bool> ExistsByIdAsync(Guid id, Guid userId, CancellationToken cancellationToken)
        => _myFinanceDbContext.Set<TUserOwnedEntity>()
            .AnyAsync(entity => entity.Id == id && entity.UserId == userId, cancellationToken);

    public void Insert(TUserOwnedEntity entity)
        => _myFinanceDbContext.Set<TUserOwnedEntity>().Add(entity);

    public void Update(TUserOwnedEntity entity)
        => _myFinanceDbContext.Set<TUserOwnedEntity>().Update(entity);

    public void Delete(TUserOwnedEntity entity)
        => _myFinanceDbContext.Set<TUserOwnedEntity>().Remove(entity);
}
