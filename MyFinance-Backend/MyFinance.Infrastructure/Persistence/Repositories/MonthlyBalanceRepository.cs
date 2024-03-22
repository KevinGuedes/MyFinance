using Microsoft.EntityFrameworkCore;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Domain.Entities;
using MyFinance.Infrastructure.Persistence.Context;

namespace MyFinance.Infrastructure.Persistence.Repositories;

internal sealed class MonthlyBalanceRepository(MyFinanceDbContext myFinanceDbContext)
    : EntityRepository<MonthlyBalance>(myFinanceDbContext), IMonthlyBalanceRepository
{
    public Task<MonthlyBalance?> GetByReferenceDateAndBusinessUnitId(
        DateTime referenceDate,
        Guid businessUnitId,
        CancellationToken cancellationToken)
        => _myFinanceDbContext.MonthlyBalances
            .FirstOrDefaultAsync(
                mb => mb.ReferenceYear == referenceDate.Year &&
                      mb.ReferenceMonth == referenceDate.Month &&
                      mb.BusinessUnitId == businessUnitId,
                cancellationToken);

    public async Task<IEnumerable<MonthlyBalance>> GetPaginatedByBusinessUnitIdAsync(
        Guid businessUnitId,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken)
        => await _myFinanceDbContext.MonthlyBalances
            .Where(mb => mb.BusinessUnitId == businessUnitId)
            .OrderByDescending(mb => mb.ReferenceYear)
            .ThenByDescending(mb => mb.ReferenceMonth)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

    public Task<MonthlyBalance?> GetWithSummaryData(
        Guid id,
        CancellationToken cancellationToken)
        => _myFinanceDbContext.MonthlyBalances
            .Include(mb => mb.BusinessUnit)
            .Include(mb => mb.Transfers
                .OrderByDescending(t => t.CreatedOnUtc)
                .ThenByDescending(t => t.RelatedTo))
            .ThenInclude(t => t.AccountTag)
            .AsNoTracking()
            .FirstOrDefaultAsync(mb => mb.Id == id, cancellationToken);
}