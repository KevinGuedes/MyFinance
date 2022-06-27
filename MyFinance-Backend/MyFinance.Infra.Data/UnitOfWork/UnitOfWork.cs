using MyFinance.Infra.Data.Context;

namespace MyFinance.Infra.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IMongoContext _mongoContext;

        public UnitOfWork(IMongoContext mongoContext)
            => _mongoContext = mongoContext;

        public Task<bool> CommitAsync(CancellationToken cancellationToken)
            => _mongoContext.SaveChangesAsync(cancellationToken);
    }
}
