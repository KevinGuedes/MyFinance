using MyFinance.Domain.Entities;
using MyFinance.Domain.ValueObjects;

namespace MyFinance.Domain.Interfaces
{
    public interface IMonthlyBalanceRepository : IEntityRepository<MonthlyBalance>
    {
        Task<IEnumerable<MonthlyBalance>> GetByBusinessUnitId(Guid businessUnitId, int count, int skip, CancellationToken cancellationToken);
        Task<MonthlyBalance> GetByReferenceData(ReferenceData referenceData, CancellationToken cancellationToken);
    }
}
