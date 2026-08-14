namespace MyFinance.TestCommon.Common;

public static class DataGenerator
{
    public static Faker CreateFaker(int? seed = null)
    {
        var faker = new Faker();

        if (seed is not null)
            faker.Random = new Randomizer(seed.Value);

        return faker;
    }
}
