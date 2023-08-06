using Microsoft.EntityFrameworkCore;
using MyFinance.Domain.Entities;
using MyFinance.Domain.Interfaces;
using MyFinance.Infra.Data.Context;

namespace MyFinance.Infra.Data.Repositories;

public sealed class MonthlyBalanceRepository : EntityRepository<MonthlyBalance>, IMonthlyBalanceRepository
{
    public MonthlyBalanceRepository(MyFinanceDbContext myFinanceDbContext) : base(myFinanceDbContext)
    {
    }

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
        int page,
        int pageSize,
        CancellationToken cancellationToken)
        => await _myFinanceDbContext.MonthlyBalances
            .Where(mb => mb.BusinessUnitId == businessUnitId)
            .OrderByDescending(mb => mb.ReferenceYear)
            .ThenByDescending(mb => mb.ReferenceMonth)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

    public Task<MonthlyBalance?> GetWithSummaryData(Guid id, CancellationToken cancellationToken)
       => _myFinanceDbContext.MonthlyBalances
            .Include(mb => mb.BusinessUnit)
            .Include(mb => mb.Transfers
                .OrderByDescending(t => t.CreationDate)
                .ThenByDescending(t => t.RelatedTo))
            .AsNoTracking()
            .AsSplitQuery()
            .FirstOrDefaultAsync(mb => mb.Id == id, cancellationToken);
}
