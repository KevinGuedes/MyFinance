using Microsoft.EntityFrameworkCore.Storage;
using MyFinance.Infra.Data.Context;
using System.Data;
using System.Data.Common;

namespace MyFinance.Infra.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MyFinanceDbContext _myFinanceDbContext;
        private IDbTransaction? _dbTransaction;

        public UnitOfWork(MyFinanceDbContext myFinanceDbContext)
            => _myFinanceDbContext = myFinanceDbContext;

        public async Task BeginTrasactionAsync(CancellationToken cancellationToken)
        {
            var transactionInitialization = await _myFinanceDbContext.Database.BeginTransactionAsync(cancellationToken);
            _dbTransaction = transactionInitialization.GetDbTransaction();
        }

        public void CommitTransaction()
        {
            _dbTransaction?.Commit();
        }

        public void RollbackTransaction()
        {
            _dbTransaction?.Rollback();
        }

        public Task SaveChangesAsync(CancellationToken cancellationToken)
            => _myFinanceDbContext.SaveChangesAsync(cancellationToken);
    }
}
