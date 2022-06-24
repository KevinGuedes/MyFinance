using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MyFinance.Domain.Entities;
using MyFinance.Domain.Enums;

namespace MyFinance.Infra.Data.Mappers
{
    internal static class TransferMapper
    {
        internal static void Map()
            => BsonClassMap.RegisterClassMap<Transfer>(map =>
            {
                map.AutoMap();
                map.MapMember(transfer => transfer.Type)
                    .SetSerializer(new EnumSerializer<TransferType>(BsonType.String))
                    .SetIsRequired(true);
            });
    }
}
