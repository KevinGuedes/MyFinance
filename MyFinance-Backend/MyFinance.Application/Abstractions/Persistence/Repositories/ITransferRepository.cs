using MyFinance.Domain.Entities;

namespace MyFinance.Application.Abstractions.Persistence.Repositories;

public interface ITransferRepository
{
    Task<Transfer?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    void Update(Transfer transfer);
    void Insert(Transfer transfer);
    void Delete(Transfer transfer);
}