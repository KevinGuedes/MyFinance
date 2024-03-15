using MyFinance.Application.Abstractions.Data;
using MyFinance.Infrastructure.Data.Context;

namespace MyFinance.Infrastructure.Data.UnitOfWork;

public sealed class UnitOfWork(MyFinanceDbContext myFinanceDbContext) : IUnitOfWork
{
    private readonly MyFinanceDbContext _myFinanceDbContext = myFinanceDbContext;

    public Task SaveChangesAsync(CancellationToken cancellationToken)
        => _myFinanceDbContext.SaveChangesAsync(cancellationToken);

    public bool HasChanges()
        => _myFinanceDbContext.ChangeTracker.HasChanges();
}
