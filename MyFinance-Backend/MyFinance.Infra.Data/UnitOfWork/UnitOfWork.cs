using MyFinance.Infra.Data.Context;

namespace MyFinance.Infra.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MyFinanceDbContext _myFinanceDbContext;

        public UnitOfWork(MyFinanceDbContext myFinanceDbContext)
            => _myFinanceDbContext = myFinanceDbContext;

        public Task BeginTrasactionAsync(CancellationToken cancellationToken)
            => _myFinanceDbContext.Database.BeginTransactionAsync(cancellationToken);

        public Task CommitTransactionAsync(CancellationToken cancellationToken)
            => _myFinanceDbContext.Database.CommitTransactionAsync(cancellationToken);

        public Task RollbackTransactionAsync(CancellationToken cancellationToken)
            => _myFinanceDbContext.Database.RollbackTransactionAsync(cancellationToken);

        public Task SaveChangesAsync(CancellationToken cancellationToken)
            => _myFinanceDbContext.SaveChangesAsync(cancellationToken);
    }
}
