using Microsoft.EntityFrameworkCore;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Domain.Entities;
using MyFinance.Infrastructure.Persistence.Context;

namespace MyFinance.Infrastructure.Persistence.Repositories;

internal sealed class TransferRepository(MyFinanceDbContext myFinanceDbContext)
    : EntityRepository<Transfer>(myFinanceDbContext), ITransferRepository
{
    public async Task<Transfer?> GetWithBusinessUnitByIdAsync(Guid id, CancellationToken cancellationToken)
        => await _myFinanceDbContext.Transfers
            .Include(transfer => transfer.BusinessUnit)
            .FirstOrDefaultAsync(transfer => transfer.Id == id, cancellationToken);

    public async Task<IEnumerable<Transfer>> GetByParamsAsync(
        Guid businessUnitId,
        DateOnly? from,
        DateOnly? to,
        Guid? categoryId,
        Guid? accountTagId,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken)
    {
        var transfers = _myFinanceDbContext.Transfers
            .AsNoTracking()
            .Where(transfer => transfer.BusinessUnitId == businessUnitId);

        if (from.HasValue && from.Value != default)
        {
            var fromDateTimeInStartOfDay = new DateTime(from.Value, new TimeOnly()); 
            transfers = transfers.Where(transfer => transfer.SettlementDate >= fromDateTimeInStartOfDay);
        }

        if (to.HasValue && to.Value != default)
        {
            var toDateTimeInEndOfDay = new DateTime(to.Value, new TimeOnly()).AddDays(1).AddTicks(-1);
            transfers = transfers.Where(transfer => transfer.SettlementDate <= toDateTimeInEndOfDay);
        }

        if (categoryId.HasValue && categoryId.Value != default)
            transfers = transfers.Where(transfer => transfer.CategoryId == categoryId.Value);

        if (accountTagId.HasValue && accountTagId.Value != default)
            transfers = transfers.Where(transfer => transfer.AccountTagId == accountTagId.Value);

        return await transfers
            .OrderByDescending(transfer => transfer.SettlementDate)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }
}