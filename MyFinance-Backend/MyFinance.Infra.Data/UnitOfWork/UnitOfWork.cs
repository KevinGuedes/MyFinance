using MyFinance.Infra.Data.Context;

namespace MyFinance.Infra.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IMongoContext _mongoContext;

        public UnitOfWork(IMongoContext mongoContext)
            => _mongoContext = mongoContext;

        public async Task<bool> CommitAsync(CancellationToken cancellationToken)
        {
            var changesCount= await _mongoContext.SaveChangesAsync(cancellationToken);
            return changesCount > 0;
        }
    }
}
