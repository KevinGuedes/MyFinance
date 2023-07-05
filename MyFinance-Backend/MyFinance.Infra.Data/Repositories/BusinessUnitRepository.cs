using MyFinance.Domain.Entities;
using MyFinance.Domain.Interfaces;
using MyFinance.Infra.Data.Context;

namespace MyFinance.Infra.Data.Repositories
{
    public class BusinessUnitRepository : EntityRepository<BusinessUnit>, IBusinessUnitRepository
    {
        public BusinessUnitRepository(MyFinanceDbContext myFinanceDbContext)
            :base(myFinanceDbContext) { }

        public Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
