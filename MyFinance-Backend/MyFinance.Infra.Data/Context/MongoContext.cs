using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MyFinance.Infra.Data.Settings;

namespace MyFinance.Infra.Data.Context
{
    public class MongoContext : IMongoContext
    {
        private readonly IMongoDatabase _database;
        private readonly IMongoClient _mongoClient;
        private readonly List<Func<IClientSessionHandle, CancellationToken, Task>> _commands;

        public MongoContext(IMongoClient mongoClient, IOptions<MongoSettings> mongoSettings)
        {
            _database = mongoClient.GetDatabase(mongoSettings.Value.DatabaseName);
            _mongoClient = mongoClient;
            _commands = new List<Func<IClientSessionHandle, CancellationToken, Task>>();
        }

        public IMongoCollection<TEntity> GetCollection<TEntity>()
            => _database.GetCollection<TEntity>(typeof(TEntity).Name);

        public void AddCommand(Func<IClientSessionHandle, CancellationToken, Task> command)
            => _commands.Add(command);

        public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken)
        {
            using var session = await _mongoClient.StartSessionAsync(cancellationToken: cancellationToken);
            var changesCount = await session.WithTransactionAsync(
                async (session, cancellationToken) =>
                {
                    var commandsTasks = _commands.Select(command => command(session, cancellationToken));
                    await Task.WhenAll(commandsTasks);
                    return _commands.Count;
                },
                cancellationToken: cancellationToken);

            return changesCount > 0;
        }
    }
}
