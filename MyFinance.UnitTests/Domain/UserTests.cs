using MyFinance.TestCommon.Factories;

namespace MyFinance.UnitTests.Domain;

public sealed class UserTests
{
    [Fact]
    public void User_ShouldSetANewSecurityStamp_WhenUpdateSecurityStampIsCalled()
    {
        var user = UserFactory.CreateUser();
        var initialSecurityStamp = user.SecurityStamp;

        user.UpdateSecurityStamp();

        Assert.NotEqual(user.SecurityStamp, initialSecurityStamp);
    }
}
