using Microsoft.EntityFrameworkCore;
using MyFinance.Domain.Entities;
using MyFinance.Domain.Interfaces;
using MyFinance.Domain.ValueObjects;
using MyFinance.Infra.Data.Context;

namespace MyFinance.Infra.Data.Repositories
{
    public class MonthlyBalanceRepository : EntityRepository<MonthlyBalance>, IMonthlyBalanceRepository
    {
        public MonthlyBalanceRepository(MyFinanceDbContext myFinanceDbContext)
            : base(myFinanceDbContext) { }

        public async Task<IEnumerable<MonthlyBalance>> GetByBusinessUnitId(Guid businessUnitId, int count, int skip, CancellationToken cancellationToken)
            => await _myFinanceDbContext.MonthlyBalances
                .Where(mb => mb.BusinessUnitId == businessUnitId)
                .Skip(skip)
                .Take(count)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

        public async Task<MonthlyBalance?> GetByReferenceData(ReferenceData referenceData, CancellationToken cancellationToken)
           => await _myFinanceDbContext.MonthlyBalances
                .AsNoTracking()
                .FirstOrDefaultAsync(mb => mb.ReferenceData == referenceData, cancellationToken);
    }
}
