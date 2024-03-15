using Microsoft.EntityFrameworkCore;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Domain.Entities;
using MyFinance.Infrastructure.Persistence.Context;

namespace MyFinance.Infrastructure.Persistence.Repositories;

public sealed class MonthlyBalanceRepository(MyFinanceDbContext myFinanceDbContext)
    : UserOwnedEntityRepository<MonthlyBalance>(myFinanceDbContext), IMonthlyBalanceRepository
{
    public Task<MonthlyBalance?> GetByReferenceDateAndBusinessUnitId(
        DateTime referenceDate,
        Guid businessUnitId,
        Guid userId,
        CancellationToken cancellationToken)
        => _myFinanceDbContext.MonthlyBalances
            .Where(mb => mb.UserId == userId)
            .FirstOrDefaultAsync(
                mb => mb.ReferenceYear == referenceDate.Year &&
                mb.ReferenceMonth == referenceDate.Month &&
                mb.BusinessUnitId == businessUnitId,
                cancellationToken);

    public async Task<IEnumerable<MonthlyBalance>> GetPaginatedByBusinessUnitIdAsync(
        Guid businessUnitId,
        int page,
        int pageSize,
        Guid userId,
        CancellationToken cancellationToken)
        => await _myFinanceDbContext.MonthlyBalances
            .Where(mb => mb.BusinessUnitId == businessUnitId && mb.UserId == userId)
            .OrderByDescending(mb => mb.ReferenceYear)
            .ThenByDescending(mb => mb.ReferenceMonth)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

    public Task<MonthlyBalance?> GetWithSummaryData(
        Guid id,
        Guid userId,
        CancellationToken cancellationToken)
       => _myFinanceDbContext.MonthlyBalances
            .Where(mb => mb.UserId == userId)
            .Include(mb => mb.BusinessUnit)
            .Include(mb => mb.Transfers
                .OrderByDescending(t => t.CreationDate)
                .ThenByDescending(t => t.RelatedTo))
            .ThenInclude(t => t.AccountTag)
            .AsNoTracking()
            .FirstOrDefaultAsync(mb => mb.Id == id, cancellationToken);
}
