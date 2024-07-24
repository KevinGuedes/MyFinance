using MyFinance.Domain.Entities;

namespace MyFinance.Application.Abstractions.Persistence.Repositories;

public interface ITransferRepository
{
    Task<IEnumerable<(int Year, int Month, decimal Income, decimal Outcome)>> GetDiscriminatedBalanceDataAsync(
        Guid managementUnitId,
        int pastMonths,
        bool includeCurrentMonth,
        CancellationToken cancellationToken);
    Task<(decimal Income, decimal Outcome)> GetBalanceDataFromPeriodAsync(
        Guid managementUnitId,
        DateTime? startDate,
        DateTime? endDate,
        Guid? categoryId,
        Guid? accountTagId,
        CancellationToken cancellationToken);
    Task<(long TotalCount, IEnumerable<(DateOnly Date, IEnumerable<Transfer> Transfers, decimal Income, decimal Outcome)> TransferGroups)> GetTransferGroupsAsync(
        int month,
        int year,
        Guid managementUnitId,
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