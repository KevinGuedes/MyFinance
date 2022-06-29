using MongoDB.Driver;

namespace MyFinance.Infra.Data.Context
{
    public interface IMongoContext
    {
        IMongoCollection<TEntity> GetCollection<TEntity>();
        void AddCommand(Func<IClientSessionHandle, CancellationToken, Task> command);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
