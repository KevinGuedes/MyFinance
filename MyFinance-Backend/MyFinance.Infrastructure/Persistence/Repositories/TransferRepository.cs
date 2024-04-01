using Microsoft.EntityFrameworkCore;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Domain.Entities;
using MyFinance.Domain.Enums;
using MyFinance.Infrastructure.Persistence.Context;

namespace MyFinance.Infrastructure.Persistence.Repositories;

internal sealed class TransferRepository(MyFinanceDbContext myFinanceDbContext)
    : EntityRepository<Transfer>(myFinanceDbContext), ITransferRepository
{
    public async Task<Transfer?> GetWithBusinessUnitByIdAsync(Guid id, CancellationToken cancellationToken)
        => await _myFinanceDbContext.Transfers
            .Include(transfer => transfer.BusinessUnit)
            .FirstOrDefaultAsync(transfer => transfer.Id == id, cancellationToken);

    public async Task<Tuple<decimal, decimal>> GetIncomeAndOutcomeFromPeriodByParams(
        Guid businessUnitId,
        DateOnly? startDate,
        DateOnly? endDate,
        Guid? categoryId,
        Guid? accountTagId,
        CancellationToken cancellationToken)
    {
        var transfers = GetByParams(
            businessUnitId,
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

        return new Tuple<decimal, decimal>(income, outcome);
    }

    public async Task<IEnumerable<Transfer>> GetTransfersByParams(
        Guid businessUnitId,
        DateOnly? startDate,
        DateOnly? endDate,
        Guid? categoryId,
        Guid? accountTagId,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken)
    {
        var transfers = GetByParams(
            businessUnitId,
            startDate,
            endDate,
            categoryId,
            accountTagId);

        return await transfers
            .OrderByDescending(transfer => transfer.SettlementDate)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    private IQueryable<Transfer> GetByParams(
        Guid businessUnitId,
        DateOnly? startDate,
        DateOnly? endDate,
        Guid? categoryId,
        Guid? accountTagId)
    {
        var transfers = _myFinanceDbContext.Transfers
            .AsNoTracking()
            .Where(transfer => transfer.BusinessUnitId == businessUnitId);

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
}