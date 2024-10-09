namespace MyFinance.TestCommon.Extensions;

public static class FakerExtensions
{
    /// <summary>
    /// Allows <see cref="Faker"/> to use private constructors. 
    /// For more details: https://github.com/bchavez/Bogus/issues/213
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="faker"></param>
    /// <returns>The <see cref="Faker"/> instance</returns>
    public static Faker<T> UsePrivateConstructor<T>(this Faker<T> faker) where T : class
        => faker.CustomInstantiator(_ =>
        {
            var instance = Activator.CreateInstance(typeof(T), true);
            return (instance as T)!;
        });
}
