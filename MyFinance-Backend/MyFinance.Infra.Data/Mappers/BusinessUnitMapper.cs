using MongoDB.Bson.Serialization;
using MyFinance.Domain.Entities;

namespace MyFinance.Infra.Data.Mappers
{
    internal class BusinessUnitMapper
    {
        internal static void Map()
            => BsonClassMap.RegisterClassMap<BusinessUnit>(map => map.AutoMap());
    }
}
