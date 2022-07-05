using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MyFinance.Domain.Entities;
using MyFinance.Domain.Interfaces;
using MyFinance.Infra.Data.Context;

namespace MyFinance.Infra.Data.Repositories
{
    public class BusinessUnitRepository : EntityRepository<BusinessUnit>, IBusinessUnitRepository
    {
        public BusinessUnitRepository(IMongoContext mongoDbContext) : base(mongoDbContext)
        {
        }

        public Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken)
            => _collection.AsQueryable()
                .AnyAsync(businessUnit => businessUnit.Name.ToLower() == name.ToLower(), cancellationToken);
    }
}
