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

        public Task<IEnumerable<MonthlyBalance>> GetByBusinessUnitId(Guid businessUnitId, int count, int skip, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<MonthlyBalance> GetByReferenceData(ReferenceData referenceData, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
