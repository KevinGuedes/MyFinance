using MyFinance.TestCommon.Common;

namespace MyFinance.TestCommon.Builders.Users;

public class UserBuilder : EntityBuilder<User>
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

    public override User Build()
    {
        var values = new
        {
            Id = _id,
            CreatedOnUtc = _createdOnUtc,
            UpdatedOnUtc = _updatedOnUtc,
            Name = _name,
            Email = _email,
            PasswordHash = _passwordHash,
            FailedSignInAttempts = _failedSignInAttempts,
            IsEmailVerified = _isEmailVerified,
            LastPasswordUpdateOnUtc = _lastPasswordUpdateOnUtc,
            LockoutEndOnUtc = _lockoutEndOnUtc,
            SecurityStamp = _securityStamp
        };

        return BuildEntity(values);
    }
}