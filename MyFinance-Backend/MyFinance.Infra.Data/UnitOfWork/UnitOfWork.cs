using MyFinance.Infra.Data.Context;

namespace MyFinance.Infra.Data.UnitOfWork;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly MyFinanceDbContext _myFinanceDbContext;

    public UnitOfWork(MyFinanceDbContext myFinanceDbContext)
        => _myFinanceDbContext = myFinanceDbContext;

    public Task SaveChangesAsync(CancellationToken cancellationToken)
        => _myFinanceDbContext.SaveChangesAsync(cancellationToken);
}
