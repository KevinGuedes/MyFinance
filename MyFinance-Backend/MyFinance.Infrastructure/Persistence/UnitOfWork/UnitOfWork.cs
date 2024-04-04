using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MyFinance.Application.Abstractions.Persistence.UnitOfWork;
using MyFinance.Infrastructure.Persistence.Context;
using System.Data;

namespace MyFinance.Infrastructure.Persistence.UnitOfWork;

internal sealed class UnitOfWork(MyFinanceDbContext myFinanceDbContext) : IUnitOfWork
{
    private readonly MyFinanceDbContext _myFinanceDbContext = myFinanceDbContext;

    public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken)
        => _myFinanceDbContext.Database
            .BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
    
    public Task SaveChangesAsync(CancellationToken cancellationToken)
        => _myFinanceDbContext.SaveChangesAsync(cancellationToken);

    public bool HasChanges()
        => _myFinanceDbContext.ChangeTracker.HasChanges();
}