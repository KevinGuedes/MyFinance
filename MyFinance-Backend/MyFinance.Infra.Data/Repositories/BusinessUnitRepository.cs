using Microsoft.EntityFrameworkCore;
using MyFinance.Domain.Entities;
using MyFinance.Domain.Interfaces;
using MyFinance.Infra.Data.Context;

namespace MyFinance.Infra.Data.Repositories;

public class BusinessUnitRepository : EntityRepository<BusinessUnit>, IBusinessUnitRepository
{
    public BusinessUnitRepository(MyFinanceDbContext myFinanceDbContext)
        : base(myFinanceDbContext) { }

    public Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken)
        => _myFinanceDbContext.BusinessUnits.AnyAsync(bu => bu.Name == name, cancellationToken);

    public async Task<BusinessUnit?> GetByNameAsync(string name, CancellationToken cancellationToken)
        => await _myFinanceDbContext.BusinessUnits.Where(bu => bu.Name == name).FirstOrDefaultAsync(cancellationToken);
}
