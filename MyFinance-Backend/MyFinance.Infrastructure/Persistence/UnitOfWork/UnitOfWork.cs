using Microsoft.EntityFrameworkCore;
using MyFinance.Application.Abstractions.Persistence.UnitOfWork;
using MyFinance.Infrastructure.Persistence.Context;
using System.Data;

namespace MyFinance.Infrastructure.Persistence.UnitOfWork;

internal sealed class UnitOfWork(MyFinanceDbContext myFinanceDbContext) : IUnitOfWork
{
    private readonly MyFinanceDbContext _myFinanceDbContext = myFinanceDbContext;

    public Task BeginTransactionAsync(CancellationToken cancellationToken)
        => _myFinanceDbContext.Database
            .BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);

    public Task CommitTransactionAsync(CancellationToken cancellationToken)
        => _myFinanceDbContext.Database
            .CommitTransactionAsync(cancellationToken);

    public Task RollbackTransactionAsync(CancellationToken cancellationToken)
        => _myFinanceDbContext.Database
            .RollbackTransactionAsync(cancellationToken);

    public bool HasChanges()
        => _myFinanceDbContext.ChangeTracker.HasChanges();
}