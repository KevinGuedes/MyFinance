using MyFinance.TestCommon.Builders.Users;
using MyFinance.TestCommon.Common;
using BC = BCrypt.Net.BCrypt;

namespace MyFinance.UnitTests.Domain;

public sealed class UserTests
{
    [Fact]
    public void User_ShouldHaveDefaultValues_WhenCreated()
    {
        var faker = DataGenerator.CreateFaker();
        var passwordHash = BC.EnhancedHashPassword(faker.Internet.Password(), 9);
        var name = faker.Person.FullName;
        var email = faker.Person.Email;
        var user = new User(name, email, passwordHash);

        Assert.Equal(name, user.Name);
        Assert.Equal(email, user.Email);
        Assert.Equal(passwordHash, user.PasswordHash);
        Assert.Equal(0, user.FailedSignInAttempts);
        Assert.False(user.IsEmailVerified);
        Assert.Null(user.LastPasswordUpdateOnUtc);
        Assert.Null(user.LockoutEndOnUtc);
        Assert.NotEqual(Guid.Empty, user.SecurityStamp);
    }

    [Fact]
    public void User_ShouldIncrementFailedSignInAttempts_WhenIncrementFailedSignInAttemptsIsCalled()
    {
        var user = UserDirector.CreateActiveUser();
        var initialFailedSignInAttempts = user.FailedSignInAttempts;

        user.IncrementFailedSignInAttempts();

        Assert.Equal(initialFailedSignInAttempts + 1, user.FailedSignInAttempts);
    }

    [Fact]
    public void User_ShouldSetALockoutEndDate_WhenSetLockoutEndIsCalled()
    {
        var user = UserDirector.CreateActiveUser();
        var lockoutEndOnUtc = DateTime.UtcNow.AddMinutes(5);

        user.SetLockoutEnd(lockoutEndOnUtc);

        Assert.Equal(lockoutEndOnUtc, user.LockoutEndOnUtc);
    }

    [Fact]
    public void User_ShouldResetLockoutData_WhenResetLockoutIsCalled()
    {
        var user = UserDirector.CreateActiveUser();
        user.IncrementFailedSignInAttempts();
        user.SetLockoutEnd(DateTime.UtcNow.AddMinutes(5));

        user.ResetLockout();

        Assert.Equal(0, user.FailedSignInAttempts);
        Assert.Null(user.LockoutEndOnUtc);
    }

    [Fact]
    public void User_ShouldUpdatePasswordHash_WhenUpdatePasswordHashIsCalled()
    {
        var faker = DataGenerator.CreateFaker();
        var user = UserDirector.CreateActiveUser();
        var initialSecurityStamp = user.SecurityStamp;
        var initialLastPasswordUpdateOnUtc = user.LastPasswordUpdateOnUtc;
        var newPasswordHash = BC.EnhancedHashPassword(faker.Internet.Password(), 9);

        user.UpdatePasswordHash(newPasswordHash);

        Assert.Equal(newPasswordHash, user.PasswordHash);
        Assert.NotEqual(initialSecurityStamp, user.SecurityStamp);
        Assert.NotEqual(initialLastPasswordUpdateOnUtc, user.LastPasswordUpdateOnUtc);
    }

    [Fact]
    public void User_ShouldHaveEmailVerified_WhenVerifyEmailIsCalled()
    {
        var user = UserBuilder.With()
            .Id(Guid.NewGuid())
            .IsEmailVerified(false)
            .Build();

        user.VerifyEmail();

        Assert.True(user.IsEmailVerified);
    }

    [Fact]
    public void User_ShouldSetANewSecurityStamp_WhenUpdateSecurityStampIsCalled()
    {
        var user = UserDirector.CreateActiveUser();
        var initialSecurityStamp = user.SecurityStamp;

        user.UpdateSecurityStamp();

        Assert.NotEqual(initialSecurityStamp, user.SecurityStamp);
    }
}
