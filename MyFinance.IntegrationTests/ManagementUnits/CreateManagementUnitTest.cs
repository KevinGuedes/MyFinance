using MyFinance.Application.UseCases.ManagementUnits.Commands.CreateManagementUnit;
using MyFinance.Application.UseCases.ManagementUnits.Queries.GetManagementUnit;
using MyFinance.Contracts.ManagementUnit.Requests;
using MyFinance.Contracts.ManagementUnit.Responses;
using MyFinance.IntegrationTests.Common;
using System.Net.Http.Json;
using System.Text.Json;

namespace MyFinance.IntegrationTests.ManagementUnits;

public class CreateManagementUnitTest(ApplicationFactory factory) : BaseIntegrationTest(factory)
{
    private const string _baseUrl = "/managementunit";

    [Fact]
    public async Task CreateManagementUnitHandle_Should_CreateManagementUnit()
    {
        var request = new CreateManagementUnitRequest("Test MU", Description: "Test Description");
       
        var (response, managementUnit) = await PostAsync<CreateManagementUnitRequest, ManagementUnitResponse>(_baseUrl, request);
       
        response.EnsureSuccessStatusCode();
        Assert.NotNull(managementUnit);
        Assert.NotEqual(Guid.Empty, managementUnit.Id);
        Assert.Equal(request.Name, managementUnit.Name);
        Assert.Equal(request.Description, managementUnit.Description);
        Assert.Equal(0, managementUnit.Income);
        Assert.Equal(0, managementUnit.Outcome);
        Assert.Equal(0, managementUnit.Balance);

        //var getRequests = new GetManagementUnitQuery(managementUnit.Id);
        //var getResponse = await Sender.Send(getRequests);
        //getResponse.IsSuccess.Should().BeTrue();
        //Assert.NotNull(getResponse.Value);
    }
}
