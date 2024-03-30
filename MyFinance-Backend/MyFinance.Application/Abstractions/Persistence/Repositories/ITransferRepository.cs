using MyFinance.Domain.Entities;

namespace MyFinance.Application.Abstractions.Persistence.Repositories;

public interface ITransferRepository
{
    Task<IEnumerable<Transfer>> GetByParamsAsync(
        Guid businessUnitId,
        DateOnly? startDate,
        DateOnly? endDate,
        Guid? categoryId,
        Guid? accountTagId,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken);
    Task<Transfer?> GetWithBusinessUnitByIdAsync(Guid id, CancellationToken cancellationToken);
    void Update(Transfer transfer);
    void Insert(Transfer transfer);
    void Delete(Transfer transfer);
}