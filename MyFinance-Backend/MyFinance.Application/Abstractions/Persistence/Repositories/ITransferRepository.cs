using MyFinance.Domain.Entities;

namespace MyFinance.Application.Abstractions.Persistence.Repositories;

public interface ITransferRepository
{
    Task<IEnumerable<(int Month, decimal Income, decimal Outcome)>> GetDiscriminatedAnnualBalanceDataAsync(
        Guid managementUnitId,
        int year,
        CancellationToken cancellationToken);
    Task<(decimal Income, decimal Outcome)> GetBalanceDataFromPeriodAsync(
        Guid managementUnitId,
        DateOnly? startDate,
        DateOnly? endDate,
        Guid? categoryId,
        Guid? accountTagId,
        CancellationToken cancellationToken);
    Task<(long TotalCount, IEnumerable<Transfer> Transfers)> GetTransfersByParamsAsync(
        Guid managementUnitId,
        DateOnly? startDate,
        DateOnly? endDate,
        Guid? categoryId,
        Guid? accountTagId,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken);
    Task<IEnumerable<Transfer>> GetWithSummaryDataAsync(
        Guid managementUnitId,
        int year,
        int month,
        CancellationToken cancellationToken);
    Task<Transfer?> GetWithManagementUnitByIdAsync(Guid id, CancellationToken cancellationToken);
    Task InsertAsync(Transfer transfer, CancellationToken cancellationToken);
    void Update(Transfer transfer);
    void Delete(Transfer transfer);
}