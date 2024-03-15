namespace MyFinance.Application.Abstractions.Data;

public interface IUnitOfWork
{
    Task SaveChangesAsync(CancellationToken cancellationToken);
    bool HasChanges();
}
