﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MyFinance.Domain.ValueObjects;

namespace MyFinance.Infra.Data.Mappers
{
    internal static class ReferenceDataMapper
    {
        internal static void Map()
           => BsonClassMap.RegisterClassMap<ReferenceData>(map =>
           {
               map.AutoMap();
               map.MapProperty(reference => reference.Month);
               map.MapProperty(reference => reference.Year);
               map.MapMember(reference => reference.BusinessUnitId)
                   .SetSerializer(new GuidSerializer(BsonType.String));
           });
    }
}
