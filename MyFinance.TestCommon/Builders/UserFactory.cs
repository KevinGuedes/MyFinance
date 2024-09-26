using MyFinance.TestCommon.Extensions;
using System.Reflection;

namespace MyFinance.TestCommon.Factories;

public static class UserFactory
{
    private const string _userName = "Test User";
    private const string _password = "plainTextPassword123456#%";
    private const string _defaultEmail = "test-user@test.com";
    private const string _oldPasswordUserEmail = "password-to-update@gmail.com";
    private const string _passwordHash = "$2a$12$ZyHujEZLxkwOepxba1SLBOQFZrs/NlocNmqTnbHNYMe7osIbsUPdG";

    public static User DefaultTestUser
        => new(_userName, _defaultEmail, _passwordHash);

    public static (string, string) DefaultTestUserCredentials => (_defaultEmail, _password);

    public static User UserWithOldPassword
        => new(_userName, _oldPasswordUserEmail, _passwordHash);

    public static (string, string) UserWithOldPasswordCredentials => (_oldPasswordUserEmail, _password);

    public static User CreateUser(
        Guid? id = null,
        string? name = null,
        string? email = null,
        string? passwordHash = null,
        Guid? securityStamp = null)
    {
        var user = new Faker<User>()
            .UsePrivateConstructor()
            .RuleFor(user => user.Id, faker => id ?? faker.Random.Guid())
            .RuleFor(user => user.Name, faker => name ?? faker.Person.FullName)
            .RuleFor(user => user.Email, faker => email ?? faker.Person.Email)
            .RuleFor(user => user.PasswordHash, faker => passwordHash ?? faker.Internet.Password());

        return user;
    }
}
