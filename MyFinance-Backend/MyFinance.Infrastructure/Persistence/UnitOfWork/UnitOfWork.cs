using MyFinance.Application.Abstractions.Persistence.UnitOfWork;
using MyFinance.Infrastructure.Persistence.Context;

namespace MyFinance.Infrastructure.Persistence.UnitOfWork;

internal sealed class UnitOfWork(MyFinanceDbContext myFinanceDbContext) : IUnitOfWork
{
    private readonly MyFinanceDbContext _myFinanceDbContext = myFinanceDbContext;

    public Task SaveChangesAsync(CancellationToken cancellationToken)
        => _myFinanceDbContext.SaveChangesAsync(cancellationToken);

    public bool HasChanges()
        => _myFinanceDbContext.ChangeTracker.HasChanges();
}