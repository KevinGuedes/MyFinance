using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MyFinance.Domain.Entities;
using MyFinance.Domain.Interfaces;
using MyFinance.Infra.Data.Context;

namespace MyFinance.Infra.Data.Repositories
{
    public class MonthlyBalanceRepository : EntityRepository<MonthlyBalance>, IMonthlyBalanceRepository
    {
        public MonthlyBalanceRepository(IMongoContext mongoContext) : base(mongoContext)
        {
        }

        public Task<MonthlyBalance> GetByMonthAndYearAsync(
            int month,
            int year,
            CancellationToken cancellationToken)
            => _collection.AsQueryable()
                .Where(montlyBalance => montlyBalance.Month == month && montlyBalance.Year == year)
                .FirstOrDefaultAsync(cancellationToken);

        public async Task<IEnumerable<MonthlyBalance>> GetByBusinessUnitId(
            Guid businessUnitId,
            int count,
            int skip,
            CancellationToken cancellationToken)
            => await _collection.AsQueryable()
                .Where(monthlyBalance => monthlyBalance.BusinessUnitId == businessUnitId)
                .OrderByDescending(monthlyBalance => monthlyBalance.CreationDate)
                .Skip(skip)
                .Take(count)
                .ToListAsync(cancellationToken);
    }
}
