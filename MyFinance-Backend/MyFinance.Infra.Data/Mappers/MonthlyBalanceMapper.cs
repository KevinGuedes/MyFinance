using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MyFinance.Domain.Entities;

namespace MyFinance.Infra.Data.Mappers
{
    internal static class MonthlyBalanceMapper
    {
        internal static void Map()
            => BsonClassMap.RegisterClassMap<MonthlyBalance>(map =>
            {
                map.AutoMap();
                map.MapMember(entity => entity.BusinessUnitId)
                    .SetSerializer(new GuidSerializer(BsonType.String));
            });
    }
}
