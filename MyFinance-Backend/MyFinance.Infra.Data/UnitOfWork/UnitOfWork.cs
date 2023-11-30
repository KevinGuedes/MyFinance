using MyFinance.Infra.Data.Context;

namespace MyFinance.Infra.Data.UnitOfWork;

public sealed class UnitOfWork(MyFinanceDbContext myFinanceDbContext) : IUnitOfWork
{
    private readonly MyFinanceDbContext _myFinanceDbContext = myFinanceDbContext;

    public Task SaveChangesAsync(CancellationToken cancellationToken)
        => _myFinanceDbContext.SaveChangesAsync(cancellationToken);
}
