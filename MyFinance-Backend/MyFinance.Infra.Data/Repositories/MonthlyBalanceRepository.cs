using Microsoft.EntityFrameworkCore;
using MyFinance.Domain.Entities;
using MyFinance.Domain.Interfaces;
using MyFinance.Infra.Data.Context;

namespace MyFinance.Infra.Data.Repositories;

public class MonthlyBalanceRepository : EntityRepository<MonthlyBalance>, IMonthlyBalanceRepository
{
    public MonthlyBalanceRepository(MyFinanceDbContext myFinanceDbContext)
        : base(myFinanceDbContext) { }

    public Task<MonthlyBalance?> GetByReferenceDateAndBusinessUnitId(DateTime referenceDate, Guid businessUnitId, CancellationToken cancellationToken)
        => _myFinanceDbContext.MonthlyBalances
            .FirstOrDefaultAsync(
                mb => mb.ReferenceYear == referenceDate.Year &&
                mb.ReferenceMonth == referenceDate.Month &&
                mb.BusinessUnitId == businessUnitId,
                cancellationToken);

}
