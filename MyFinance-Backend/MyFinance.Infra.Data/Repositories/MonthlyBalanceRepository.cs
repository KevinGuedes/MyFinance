using MyFinance.Domain.Entities;
using MyFinance.Domain.Interfaces;
using MyFinance.Infra.Data.Context;
using MyFinance.Infra.Data.UnitOfWork;
using MongoDB.Driver.Linq;
using MongoDB.Driver;

namespace MyFinance.Infra.Data.Repositories
{
    public class MonthlyBalanceRepository : EntityRepository<MonthlyBalance>, IMonthlyBalanceRepository
    {
        public MonthlyBalanceRepository(IMongoContext mongoContext, IUnitOfWork unitOfWork)
            : base(mongoContext, unitOfWork)
        {
        }

        public Task<bool> ExistsByMonthAndYearAsync(
            int month,
            int year,
            CancellationToken cancellationToken)
            => _collection.AsQueryable()
                .AnyAsync(montlyBalance => montlyBalance.Month == month && montlyBalance.Year == year, cancellationToken);

        public Task<MonthlyBalance> GetByMonthAndYearAsync(
            int month,
            int year,
            CancellationToken cancellationToken)
            => _collection.AsQueryable()
                .Where(montlyBalance => montlyBalance.Month == month && montlyBalance.Year == year)
                .FirstAsync(cancellationToken);

        public async Task<IEnumerable<MonthlyBalance>> GetAllAsync(
            int count,
            int skip,
            CancellationToken cancellationToken)
            => await _collection.AsQueryable()
                .OrderByDescending(monthlyBalance => monthlyBalance.CreationDate)
                .Skip(skip)
                .Take(count)
                .ToListAsync(cancellationToken);
    }
}
