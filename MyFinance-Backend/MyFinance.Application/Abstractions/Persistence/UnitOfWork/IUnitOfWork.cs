namespace MyFinance.Application.Abstractions.Persistence.UnitOfWork;

public interface IUnitOfWork
{
    Task SaveChangesAsync(CancellationToken cancellationToken);
    bool HasChanges();
}