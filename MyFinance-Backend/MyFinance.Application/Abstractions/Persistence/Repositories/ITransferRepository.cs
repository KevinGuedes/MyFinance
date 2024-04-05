using MyFinance.Domain.Entities;

namespace MyFinance.Application.Abstractions.Persistence.Repositories;

public interface ITransferRepository
{
    Task<Tuple<decimal, decimal>> GetIncomeAndOutcomeFromPeriodByParams(
        Guid businessUnitId,
        DateOnly? startDate,
        DateOnly? endDate,
        Guid? categoryId,
        Guid? accountTagId,
        CancellationToken cancellationToken);
    Task<Tuple<int, IEnumerable<Transfer>>> GetTransfersByParams(
        Guid businessUnitId,
        DateOnly? startDate,
        DateOnly? endDate,
        Guid? categoryId,
        Guid? accountTagId,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken);
    Task<IEnumerable<Transfer>> GetWithSummaryDataAsync(
        Guid businessUnitId,
        int year,
        int month,
        CancellationToken cancellationToken);
    Task<Transfer?> GetWithBusinessUnitByIdAsync(Guid id, CancellationToken cancellationToken);
    void Update(Transfer transfer);
    void Insert(Transfer transfer);
    void Delete(Transfer transfer);
}