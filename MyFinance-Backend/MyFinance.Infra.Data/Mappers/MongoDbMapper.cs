namespace MyFinance.Infra.Data.Mappers
{
    public static class MongoDbMapper
    {
        public static void MapEntities()
        {
            EntityMapper.Map();
            BusinessUnitMapper.Map();
            TransferMapper.Map();
            MonthlyBalanceMapper.Map();
        }
    }
}
