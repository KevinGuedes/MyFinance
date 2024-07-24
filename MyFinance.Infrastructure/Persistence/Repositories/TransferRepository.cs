using Microsoft.EntityFrameworkCore;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Domain.Entities;
using MyFinance.Domain.Enums;
using MyFinance.Infrastructure.Helpers;
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


    public async Task<(long TotalCount, IEnumerable<(DateOnly Date, IEnumerable<Transfer> Transfers, decimal Income, decimal Outcome)> TransferGroups)> GetTransferGroupsAsync(
        int month,
        int year,
        Guid managementUnitId,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken)
    {
        var startDate = DateOnly.FromDateTime(MonthHelper.GetFirstDateOfMonthInUTC(month, year));
        var endDate = DateOnly.FromDateTime(MonthHelper.GetLastDateOfMonthInUTC(month, year));

        var transfers = GetByParams(
            managementUnitId,
            startDate,
            endDate,
            null,
            null);

        var totalCount = await transfers.LongCountAsync(cancellationToken);

        var transferGroups = (await transfers
            .Include(transfer => transfer.Category)
            .Include(transfer => transfer.AccountTag)
            .OrderByDescending(transfer => transfer.SettlementDate)
            .ThenBy(transfer => transfer.RelatedTo)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .GroupBy(transfer => transfer.SettlementDate.Day)
            .Select(transferGroup => new
            {
                Date = new DateOnly(year, month, transferGroup.Key),
                Transfers = transferGroup.ToList(),
                Income = transferGroup.Sum(transfer => transfer.Type == TransferType.Profit ? transfer.Value : 0),
                Outcome = transferGroup.Sum(transfer => transfer.Type == TransferType.Expense ? transfer.Value : 0)
            })
            .ToListAsync(cancellationToken))
            .Select(transferGroups => (
                transferGroups.Date,
                Transfers: transferGroups.Transfers.AsEnumerable(),
                transferGroups.Income,
                transferGroups.Outcome
            ));
        
        return (TotalCount: totalCount, TransferGroups: transferGroups);
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
        int pastMonths,
        bool includeCurrentMonth,
        CancellationToken cancellationToken)
    {
        var (startDate, endDate) = MonthHelper.GetDateRangeInUTCFromPastMonths(pastMonths, includeCurrentMonth);
        var monthsInRange = MonthHelper.GetMonthsInRange(startDate, endDate);

        var groupedTransfers = await GetByParams(
            managementUnitId,
            DateOnly.FromDateTime(startDate),
            DateOnly.FromDateTime(endDate),
            null,
            null)
        .GroupBy(transfer => new
        {
            transfer.SettlementDate.Month,
            transfer.SettlementDate.Year
        })
        .OrderBy(transferGroup => transferGroup.Key.Year)
        .ThenBy(transferGroup => transferGroup.Key.Month)
        .Select(transferGroup => new
        {
            transferGroup.Key.Year,
            transferGroup.Key.Month,
            Income = transferGroup.Sum(transfer => transfer.Type == TransferType.Profit ? transfer.Value : 0),
            Outcome = transferGroup.Sum(transfer => transfer.Type == TransferType.Expense ? transfer.Value : 0)
        })
        .ToListAsync(cancellationToken);

        return monthsInRange
            .GroupJoin(
                groupedTransfers,
                month => new { month.Year, month.Month },
                transfer => new { transfer.Year, transfer.Month },
                (month, transfers) => new
                {
                    month.Year,
                    month.Month,
                    Income = transfers.Sum(transfer => transfer.Income),
                    Outcome = transfers.Sum(transfer => transfer.Outcome)
                })
            .Select(monthlyBalanceData => (
                monthlyBalanceData.Year,
                monthlyBalanceData.Month,
                monthlyBalanceData.Income,
                monthlyBalanceData.Outcome
            ));
    }
}