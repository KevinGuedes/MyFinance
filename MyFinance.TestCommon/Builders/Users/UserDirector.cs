using BC = BCrypt.Net.BCrypt;

namespace MyFinance.TestCommon.Builders.Users;

public static class UserDirector
{
    private const int DefaultTestUserSeed = 1;
    private const int UserWithOldPasswordSeed = 2;

    public static (User User, (string Email, string Password) Credentials) CreateDefaultTestUser()
    {
        var (randomizer, person, date, internet) = GetUserDataGenerators(DefaultTestUserSeed);
        var refDate = DateTime.UtcNow;
        var password = internet.Password();

        var user = UserBuilder.With()
            .Id(randomizer.Guid())
            .CreatedOnUtc(date.Past(3, refDate))
            .UpdatedOnUtc(date.Past(1, refDate))
            .Name(person.FullName)
            .Email(person.Email)
            .PasswordHash(BC.EnhancedHashPassword(password))
            .FailedSignInAttempts(0)
            .IsEmailVerified(false)
            .LastPasswordUpdateOnUtc(date.Recent(30))
            .SecurityStamp(randomizer.Guid())
            .Build();

        return (user, (person.Email, password));
    }

    public static (User User, (string Email, string Password) Credentials) CreateUserWithOldPassword()
    {
        var (randomizer, person, date, internet) = GetUserDataGenerators(UserWithOldPasswordSeed);
        var refDate = DateTime.UtcNow;
        var password = internet.Password();

        var user = UserBuilder.With()
            .Id(randomizer.Guid())
            .CreatedOnUtc(date.Past(2, refDate))
            .UpdatedOnUtc(date.Past(1, refDate))
            .Name(person.FullName)
            .Email(person.Email)
            .PasswordHash(BC.EnhancedHashPassword(password))
            .FailedSignInAttempts(0)
            .IsEmailVerified(true)
            .LastPasswordUpdateOnUtc(date.Past(1, refDate))
            .SecurityStamp(randomizer.Guid())
            .Build();

        return (user, (person.Email, password));
    }

    public static User CreateActiveUser()
    {
        var (randomizer, person, date, internet) = GetUserDataGenerators();

        var user = UserBuilder.With()
            .Id(randomizer.Guid())
            .CreatedOnUtc(date.Past(3))
            .UpdatedOnUtc(date.Past(1))
            .Name(person.FullName)
            .Email(person.Email)
            .PasswordHash(BC.EnhancedHashPassword(internet.Password(), 9))
            .FailedSignInAttempts(0)
            .IsEmailVerified(true)
            .LastPasswordUpdateOnUtc(date.Recent(30))
            .SecurityStamp(randomizer.Guid())
            .Build();

        return user;
    }

    private static (Randomizer Randomizer, Person Person, Date Date, Internet Internet) GetUserDataGenerators(int? seed = null)
    {
        if (seed is null)
            return new(new Randomizer(), new Person(), new Date(), new Internet());

        var randomizer = new Randomizer(seed.Value);
        var person = new Person("en", seed);

        var internet = new Internet
        {
            Random = randomizer
        };

        var date = new Date
        {
            Random = randomizer
        };

        return new(randomizer, person, date, internet);
    }
}
