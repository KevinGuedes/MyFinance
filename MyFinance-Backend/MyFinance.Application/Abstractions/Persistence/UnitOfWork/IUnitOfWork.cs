using Microsoft.EntityFrameworkCore.Storage;

namespace MyFinance.Application.Abstractions.Persistence.UnitOfWork;

public interface IUnitOfWork
{
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
    bool HasChanges();
}