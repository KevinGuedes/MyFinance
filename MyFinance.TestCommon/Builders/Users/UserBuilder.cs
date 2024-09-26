using MyFinance.Domain.Common;
using System.Reflection;
using BC = BCrypt.Net.BCrypt;

namespace MyFinance.TestCommon.Builders.Users;

public class UserBuilder : BaseEntityBuilder<User>
{
    private Guid? _id;
    private DateTime? _createdOnUtc;
    private DateTime? _updatedOnUtc;
    private string? _name;
    private string? _email;
    private string? _passwordHash;
    private int? _failedSignInAttempts;
    private bool? _isEmailVerified;
    private DateTime? _lastPasswordUpdateOnUtc;
    private DateTime? _lockoutEndOnUtc;
    private Guid? _securityStamp;

    private UserBuilder()
    {
    }

    public static UserBuilder With() => new();

    public UserBuilder Id(Guid id)
    {
        _id = id;
        return this;
    }

    public UserBuilder CreatedOnUtc(DateTime createdOnUtc)
    {
        _createdOnUtc = createdOnUtc;
        return this;
    }

    public UserBuilder UpdatedOnUtc(DateTime updatedOnUtc)
    {
        _updatedOnUtc = updatedOnUtc;
        return this;
    }

    public UserBuilder Name(string name)
    {
        _name = name;
        return this;
    }

    public UserBuilder Email(string email)
    {
        _email = email;
        return this;
    }

    public UserBuilder PasswordHash(string passwordHash)
    {
        _passwordHash = passwordHash;
        return this;
    }

    public UserBuilder FailedSignInAttempts(int failedSignInAttempts)
    {
        _failedSignInAttempts = failedSignInAttempts;
        return this;
    }

    public UserBuilder IsEmailVerified(bool isEmailVerified)
    {
        _isEmailVerified = isEmailVerified;
        return this;
    }

    public UserBuilder LastPasswordUpdateOnUtc(DateTime lastPasswordUpdateOnUtc)
    {
        _lastPasswordUpdateOnUtc = lastPasswordUpdateOnUtc;
        return this;
    }

    public UserBuilder LockoutEndOnUtc(DateTime lockoutEndOnUtc)
    {
        _lockoutEndOnUtc = lockoutEndOnUtc;
        return this;
    }

    public UserBuilder SecurityStamp(Guid securityStamp)
    {
        _securityStamp = securityStamp;
        return this;
    }

    //public User Build()
    //{
    //    var user = new Faker<User>()
    //       .UsePrivateConstructor()
    //       .RuleFor(user => user.Id, faker => _id ?? faker.Random.Guid())
    //       .RuleFor(user => user.CreatedOnUtc, faker => _createdOnUtc ?? DateTime.UtcNow)
    //       .RuleFor(user => user.UpdatedOnUtc, faker => _updatedOnUtc)
    //       .RuleFor(user => user.Name, faker => _name ?? faker.Person.FullName)
    //       .RuleFor(user => user.Email, faker => _email ?? faker.Person.Email)
    //       .RuleFor(user => user.PasswordHash, faker => _passwordHash ?? faker.Internet.Password())
    //       .RuleFor(user => user.FailedSignInAttempts, faker => _failedSignInAttempts ?? 0)
    //       .RuleFor(user => user.IsEmailVerified, faker => _isEmailVerified ?? false)
    //       .RuleFor(user => user.LastPasswordUpdateOnUtc, faker => _lastPasswordUpdateOnUtc)
    //       .RuleFor(user => user.LockoutEndOnUtc, faker => _lockoutEndOnUtc)
    //       .RuleFor(user => user.SecurityStamp, faker => _securityStamp ?? Guid.NewGuid());

    //    return user;
    //}

    //public User Build()
    //{
    //    _id = Guid.NewGuid();
    //    _createdOnUtc = DateTime.UtcNow;
    //    _updatedOnUtc = DateTime.UtcNow;
    //    _name = "Test";
    //    _email = "t@gmail.com";
    //    _failedSignInAttempts = 22;
    //    _isEmailVerified = true;

    //    var userInstance = Activator.CreateInstance(typeof(User), true);

    //    //private props?
    //    var userProps = typeof(User).GetProperties();

    //    var builderProps = GetType()
    //        .GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

    //    foreach (var builderProp in builderProps)
    //    {
    //        var builderValue = builderProp.GetValue(this);

    //        if(builderValue is null)
    //            continue;

    //        var builderPropName = builderProp.Name;
    //        var builderPropNameCleaned = builderPropName.Replace("_", string.Empty);
    //        var builderPropNameNormalized 
    //            = char.ToUpper(builderPropNameCleaned[0]) + builderPropNameCleaned[1..];

    //        var userProp = userProps
    //            .FirstOrDefault(prop => prop.Name == builderPropNameNormalized);

    //        if (userProp is null)
    //            continue;

    //        var declaringType = userProp?.DeclaringType;

    //        if (declaringType == typeof(User))
    //        {
    //            userProp!.SetValue(userInstance, builderValue);
    //        }
    //        {
    //            var originalPropInfo = declaringType!.GetProperty(userProp!.Name);
    //            originalPropInfo!.SetValue(userInstance, builderValue);
    //        }

    //    }

    //    return (userInstance as User)!;
    //}

    public User Build(bool applyConsistentValuesForEmptyProperties = true)
    {
        if (applyConsistentValuesForEmptyProperties)
        {
            var randomizer = new Randomizer();
            var personDataGenerator = new Person();
            var internetDataGenerator = new Internet();
            var dateGenerator = new Date();

            _id ??= randomizer.Guid();
            _createdOnUtc ??= dateGenerator.Past(3);
            _updatedOnUtc ??= dateGenerator.Past(1);
            _name ??= personDataGenerator.FullName;
            _email ??= personDataGenerator.Email;
            _passwordHash ??= BC.EnhancedHashPassword(internetDataGenerator.Password(), 9);
            _isEmailVerified ??= true;
            _lastPasswordUpdateOnUtc ??= dateGenerator.Recent(80);
            _failedSignInAttempts ??= 0;
            _lockoutEndOnUtc ??= null;
            _securityStamp ??= randomizer.Guid();
        }
        
        var user = BuildEntity();

        return user;
    }
}

public abstract class BaseEntityBuilder<TEntity>() where TEntity : Entity
{
    protected TEntity BuildEntity()
    {
        var builderFields = GetType()
            .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        var instance = Activator.CreateInstance(typeof(TEntity), true) ?? 
            throw new InvalidOperationException("Could not create an instance of the entity.");

        var instanceProps = instance.GetType()
            .GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        var entityProps = typeof(Entity).GetProperties();
        var createdOnUtcProp = entityProps.FirstOrDefault(prop => prop.Name == nameof(Entity.CreatedOnUtc));
        createdOnUtcProp!.SetValue(instance, null);

        foreach (var field in builderFields)
        {
            var fieldValue = field.GetValue(this);

            if (fieldValue is null || fieldValue == default)
                continue;

            var fieldName = field.Name;
            var fieldNameCleaned = fieldName.Replace("_", string.Empty);
            var fieldNameNormalized
                = char.ToUpper(fieldNameCleaned[0]) + fieldNameCleaned[1..];

            var instanceProp = instanceProps
                .FirstOrDefault(prop => prop.Name == fieldNameNormalized);

            if (instanceProp is null)
                continue;

            var declaringType = instanceProp.DeclaringType;

            if (declaringType == typeof(TEntity))
            {
                instanceProp.SetValue(instance, fieldValue);
            }
            {
                var originalPropInfo = declaringType!.GetProperty(instanceProp.Name);

                if (originalPropInfo is null)
                    continue;

                originalPropInfo.SetValue(instance, fieldValue);
            }
        }

        return (instance as TEntity)!;
    }
}