using MyFinance.Contracts.User.Requests;
using MyFinance.Contracts.User.Responses;
using MyFinance.IntegrationTests.Common;
using MyFinance.TestCommon.Factories;

namespace MyFinance.IntegrationTests.UserTests;

public class CreateUserTest(ApplicationFactory applicationFactory)
    : BaseIntegrationTest(applicationFactory, "/user")
{
    [Fact]
    public async Task CreateUserFlow_Should_ReturnAUserWithDefaultValues()
    {
        var request = new SignUpRequest(
            "Test User",
            "t@gmail.com",
            "plainTextPassword123456#%",
            "plainTextPassword123456#%");

        var response = await PostAsync(request);

        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task SingInFlow_Should_ReturnAppropriateData()
    {
        var (email, password) = UserFactory.DefaultTestUserCredentials;
        var request = new SignInRequest(email, password);

        var (response, userInfo) = await PostAsync<SignInRequest, UserInfoResponse>(request, "signin");

        response.EnsureSuccessStatusCode();
        Assert.NotNull(userInfo);
        response.Headers.Contains("Set-Cookie").Should().BeTrue();
        userInfo.ShouldUpdatePassword.Should().BeFalse();
    }

    [Fact]
    public async Task SingInFlow_WhenPasswordIsOld_Should_ReturnFlagToUpdatePassword()
    {
        var (email, password) = UserFactory.UserWithOldPasswordCredentials;
        var request = new SignInRequest(email, password);

        var (response, userInfo) = await PostAsync<SignInRequest, UserInfoResponse>(request, "signin");

        Assert.NotNull(userInfo);
        response.Headers.Contains("Set-Cookie").Should().BeTrue();
        userInfo.ShouldUpdatePassword.Should().BeTrue();
    }
}
