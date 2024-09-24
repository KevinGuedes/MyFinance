﻿namespace MyFinance.TestCommon.Extensions;

public static class FakerExtensions
{
    /// <summary>
    /// Allows <see cref="Faker"/> to use private constructors. 
    /// For mroe details: More details https://github.com/bchavez/Bogus/issues/213
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="faker"></param>
    /// <returns>The <see cref="Faker"/> instance</returns>
    public static Faker<T> UsePrivateConstructor<T>(this Faker<T> faker) where T : class
        => faker.CustomInstantiator(_ =>
        {
            var instance = Activator.CreateInstance(typeof(T), nonPublic: true);
            return (instance as T)!;
        });
}