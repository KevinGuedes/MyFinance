using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MyFinance.Domain.Entities;
using MyFinance.Domain.Interfaces;
using MyFinance.Infra.Data.Context;
using MyFinance.Infra.Data.UnitOfWork;

namespace MyFinance.Infra.Data.Repositories
{
    public class BusinessUnitRepository : EntityRepository<BusinessUnit>, IBusinessUnitRepository
    {
        public BusinessUnitRepository(IMongoContext mongoDbContext, IUnitOfWork unitOfWork)
            : base(mongoDbContext, unitOfWork)
        {
        }

        public Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken)
            => _collection.AsQueryable()
                .AnyAsync(businessUnit => businessUnit.Name.ToLower() == name.ToLower(), cancellationToken);
    }
}
