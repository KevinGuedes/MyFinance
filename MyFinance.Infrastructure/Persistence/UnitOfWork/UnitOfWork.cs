using Microsoft.EntityFrameworkCore;
using MyFinance.Application.Abstractions.Persistence;
using System.Data;

namespace MyFinance.Infrastructure.Persistence.UnitOfWork;

internal sealed class UnitOfWork(IMyFinanceDbContext myFinanceDbContext) : IUnitOfWork
{
    private readonly IMyFinanceDbContext _myFinanceDbContext = myFinanceDbContext;

    public Task BeginTransactionAsync(CancellationToken cancellationToken)
        => _myFinanceDbContext.Database
            .BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);

    public Task CommitTransactionAsync(CancellationToken cancellationToken)
        => _myFinanceDbContext.Database.CommitTransactionAsync(cancellationToken);

    public Task RollbackTransactionAsync(CancellationToken cancellationToken)
        => _myFinanceDbContext.Database.RollbackTransactionAsync(cancellationToken);

    public Task SaveChangesAsync(CancellationToken cancellationToken)
        => _myFinanceDbContext.SaveChangesAsync(cancellationToken);
}