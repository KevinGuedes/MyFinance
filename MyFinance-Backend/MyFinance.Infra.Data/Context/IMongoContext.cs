using MongoDB.Driver;

namespace MyFinance.Infra.Data.Context
{
    public interface IMongoContext
    {
        IMongoCollection<TEntity> GetCollection<TEntity>();
        void AddCommand(Func<IClientSessionHandle, CancellationToken, Task> command);
        Task<bool> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
