using System.Data;

namespace MyFinance.Infra.Data.UnitOfWork
{
    public interface IUnitOfWork
    {
        Task SaveChangesAsync(CancellationToken cancellationToken);
        Task BeginTrasactionAsync(CancellationToken cancellationToken);
        void CommitTransaction();
        void RollbackTransaction();
    }
}
