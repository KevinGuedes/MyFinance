using MyFinance.Contracts.ManagementUnit.Requests;
using MyFinance.Contracts.ManagementUnit.Responses;
using MyFinance.IntegrationTests.Common;

namespace MyFinance.IntegrationTests.ManagementUnits;

public sealed class CreateManagementUnitTest(ApplicationFactory applicationFactory)
    : BaseIntegrationTest(applicationFactory, "/managementunit")
{
    [Fact]
    public async Task CreateManagementUnitFlow_Should_ReturnAManagementUnitWithDefaultValues()
    {
        var request = new CreateManagementUnitRequest("Test MU", Description: "Test Description");

        var (response, managementUnit) = await PostAsync<CreateManagementUnitRequest, ManagementUnitResponse>(request);

        response.EnsureSuccessStatusCode();
        Assert.NotNull(managementUnit);
        Assert.NotEqual(Guid.Empty, managementUnit.Id);
        Assert.Equal(request.Name, managementUnit.Name);
        Assert.Equal(request.Description, managementUnit.Description);
        Assert.Equal(0, managementUnit.Income);
        Assert.Equal(0, managementUnit.Outcome);
        Assert.Equal(0, managementUnit.Balance);
    }
}
