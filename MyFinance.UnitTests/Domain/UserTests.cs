using MyFinance.TestCommon.Builders.Users;

namespace MyFinance.UnitTests.Domain;

public sealed class UserTests
{
    [Fact]
    public void User_ShouldSetANewSecurityStamp_WhenUpdateSecurityStampIsCalled()
    {
        var user = UserDirector.CreateActiveUser();
        var initialSecurityStamp = user.SecurityStamp;

        user.UpdateSecurityStamp();

        Assert.NotEqual(user.SecurityStamp, initialSecurityStamp);
    }
}
