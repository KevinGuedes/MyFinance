using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using MyFinance.Domain.Entities;

namespace MyFinance.Infra.Data.Mappers
{
    internal static class EntityMapper
    {
        internal static void Map()
        {
            BsonClassMap.RegisterClassMap<Entity>(map =>
            {
                map.AutoMap();
                map.SetIsRootClass(true);
                map.MapIdMember(entity => entity.Id)
                    .SetSerializer(new GuidSerializer(BsonType.String));
            });
        }
    }
}
