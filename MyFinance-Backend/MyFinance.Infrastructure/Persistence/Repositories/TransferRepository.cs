using Microsoft.EntityFrameworkCore;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Domain.Entities;
using MyFinance.Domain.Enums;
using MyFinance.Infrastructure.Persistence.Context;

namespace MyFinance.Infrastructure.Persistence.Repositories;

internal sealed class TransferRepository(MyFinanceDbContext myFinanceDbContext)
    : EntityRepository<Transfer>(myFinanceDbContext), ITransferRepository
{
    public async Task<IEnumerable<Transfer>> GetWithSummaryDataAsync(Guid managementUnitId, int year, int month, CancellationToken cancellationToken)
        => await _myFinanceDbContext.Transfers
            .AsNoTracking()
            .Where(
                transfer => transfer.SettlementDate.Year == year &&
                transfer.SettlementDate.Month == month &&
                transfer.ManagementUnitId == managementUnitId)
            .Include(transfer => transfer.Category)
            .Include(transfer => transfer.AccountTag)
            .ToListAsync(cancellationToken);

    public async Task<Transfer?> GetWithManagementUnitByIdAsync(Guid id, CancellationToken cancellationToken)
        => await _myFinanceDbContext.Transfers
            .Include(transfer => transfer.ManagementUnit)
            .FirstOrDefaultAsync(transfer => transfer.Id == id, cancellationToken);

    public async Task<(decimal Income, decimal Outcome)> GetBalanceDataFromPeriodAsync(
        Guid managementUnitId,
        DateOnly? startDate,
        DateOnly? endDate,
        Guid? categoryId,
        Guid? accountTagId,
        CancellationToken cancellationToken)
    {
        var transfers = GetByParams(
            managementUnitId,
            startDate,
            endDate,
            categoryId,
            accountTagId);

        var tranfersGroupedByType = await transfers
            .GroupBy(transfer => transfer.Type)
            .Select(transferGroup => new
            {
                Type = transferGroup.Key,
                Value = transferGroup.Sum(transfer => transfer.Value)
            })
            .ToListAsync(cancellationToken);

        var income = 0m;
        var outcome = 0m;

        foreach (var transferGroup in tranfersGroupedByType)
        {
            if (transferGroup.Type == TransferType.Profit)
                income = transferGroup.Value;

            if (transferGroup.Type == TransferType.Expense)
                outcome = transferGroup.Value;
        }

        return (Income: income, Outcome: outcome);
    }

    public async Task<(long TotalCount, IEnumerable<Transfer> Transfers)> GetTransfersByParamsAsync(
        Guid managementUnitId,
        DateOnly? startDate,
        DateOnly? endDate,
        Guid? categoryId,
        Guid? accountTagId,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken)
    {
        var transfers = GetByParams(
            managementUnitId,
            startDate,
            endDate,
            categoryId,
            accountTagId);

        var totalCount = await transfers.LongCountAsync(cancellationToken);
        var paginatedAndSortedTransfers = await transfers
            .OrderByDescending(transfer => transfer.SettlementDate)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (TotalCount: totalCount, Transfers: paginatedAndSortedTransfers);
    }

    private IQueryable<Transfer> GetByParams(
        Guid managementUnitId,
        DateOnly? startDate,
        DateOnly? endDate,
        Guid? categoryId,
        Guid? accountTagId)
    {
        var transfers = _myFinanceDbContext.Transfers
            .AsNoTracking()
            .Where(transfer => transfer.ManagementUnitId == managementUnitId);

        if (startDate.HasValue && startDate.Value != default)
        {
            var fromDateTimeInStartOfDay = new DateTime(startDate.Value, new TimeOnly());
            transfers = transfers.Where(transfer => transfer.SettlementDate >= fromDateTimeInStartOfDay);
        }

        if (endDate.HasValue && endDate.Value != default)
        {
            var toDateTimeInEndOfDay = new DateTime(endDate.Value, new TimeOnly()).AddDays(1).AddTicks(-1);
            transfers = transfers.Where(transfer => transfer.SettlementDate <= toDateTimeInEndOfDay);
        }

        if (categoryId.HasValue && categoryId.Value != default)
            transfers = transfers.Where(transfer => transfer.CategoryId == categoryId.Value);

        if (accountTagId.HasValue && accountTagId.Value != default)
            transfers = transfers.Where(transfer => transfer.AccountTagId == accountTagId.Value);

        return transfers;
    }

    public async Task<IEnumerable<(int Year, int Month, decimal Income, decimal Outcome)>> GetDiscriminatedBalanceDataAsync(
        Guid managementUnitId,
        DateTime fromDate,
        DateTime toDate,
        CancellationToken cancellationToken)
    {
        var result = await _myFinanceDbContext.Transfers
            .AsNoTracking()
            .Where(transfer =>
                transfer.SettlementDate >= fromDate &&
                transfer.SettlementDate <= toDate &&
                transfer.ManagementUnitId == managementUnitId)
            .GroupBy(transfer => new 
            { 
                transfer.SettlementDate.Month, 
                transfer.SettlementDate.Year 
            })
            .Select(transferGroup => new 
            {
                transferGroup.Key.Year,
                transferGroup.Key.Month,
                Income = transferGroup.Sum(transfer => transfer.Type == TransferType.Profit ? transfer.Value : 0),
                Outcome = transferGroup.Sum(transfer => transfer.Type == TransferType.Expense ? transfer.Value : 0)
            })
            .ToListAsync(cancellationToken);

        return result.Select(monthlyBalanceData => (
            monthlyBalanceData.Year,
            monthlyBalanceData.Month,
            monthlyBalanceData.Income,
            monthlyBalanceData.Outcome));
    }
}