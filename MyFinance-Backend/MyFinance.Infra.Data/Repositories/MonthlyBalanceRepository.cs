using Microsoft.EntityFrameworkCore;
using MyFinance.Domain.Entities;
using MyFinance.Domain.Interfaces;
using MyFinance.Infra.Data.Context;

namespace MyFinance.Infra.Data.Repositories;

public class MonthlyBalanceRepository : EntityRepository<MonthlyBalance>, IMonthlyBalanceRepository
{
    public MonthlyBalanceRepository(MyFinanceDbContext myFinanceDbContext)
        : base(myFinanceDbContext) { }

    public async Task<IEnumerable<MonthlyBalance>> GetByBusinessUnitId(Guid businessUnitId, int take, int skip, CancellationToken cancellationToken)
        => await _myFinanceDbContext.MonthlyBalances
            .Where(mb => mb.BusinessUnitId == businessUnitId)
            .OrderByDescending(mb => mb.ReferenceDate)
            .Skip(skip)
            .Take(take)
            .AsNoTracking()
            .Include(mb => mb.Transfers)
            .ToListAsync(cancellationToken);

    public Task<MonthlyBalance?> GetByReferenceDateAndBusinessUnitId(DateTime referenceDate, Guid businessUnitId, CancellationToken cancellationToken)
        => _myFinanceDbContext.MonthlyBalances
            .AsNoTracking()
            .FirstOrDefaultAsync(
                mb => mb.ReferenceDate.Year == referenceDate.Year && mb.ReferenceDate.Month == referenceDate.Month && mb.BusinessUnitId == businessUnitId, 
                cancellationToken);
       
}
