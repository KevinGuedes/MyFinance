using MongoDB.Bson.Serialization;
using MyFinance.Domain.Entities;

namespace MyFinance.Infra.Data.Mappers
{
    internal static class MonthlyBalanceMapper
    {
        internal static void Map()
            => BsonClassMap.RegisterClassMap<MonthlyBalance>(map => map.AutoMap());
    }
}
