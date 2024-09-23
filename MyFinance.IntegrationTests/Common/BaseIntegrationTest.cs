using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MyFinance.Contracts.ManagementUnit.Responses;
using MyFinance.Infrastructure.Persistence.Context;
using System.Net.Http.Json;
using System.Text.Json;

namespace MyFinance.IntegrationTests.Common;

public abstract class BaseIntegrationTest : IClassFixture<ApplicationFactory>, IDisposable
{
    private readonly IServiceScope _scope;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    protected readonly HttpClient _httpClient;
    protected readonly ISender Sender;
    protected readonly MyFinanceDbContext MyFinanceDbContext;

    protected BaseIntegrationTest(ApplicationFactory factory)
    {
        _jsonSerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        _httpClient = factory.CreateClient();
        _httpClient.DefaultRequestHeaders.Authorization = new(TestAuthenticationHandler.TestSchemeName);

        _scope = factory.Services.CreateScope();

        Sender = _scope.ServiceProvider.GetRequiredService<ISender>();
        MyFinanceDbContext = _scope.ServiceProvider.GetRequiredService<MyFinanceDbContext>();
    }

    protected async Task<(HttpResponseMessage, TResponse?)> PostAsync<TRequest, TResponse>(
        string requestUrl, 
        TRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync(requestUrl, request);
        var responseDataAsString = await response.Content.ReadAsStringAsync();
        var responseData = JsonSerializer.Deserialize<TResponse>(responseDataAsString, _jsonSerializerOptions);

        return (response, responseData);
    }

    public void Dispose()
    {
        _scope?.Dispose();
        MyFinanceDbContext?.Dispose();
        GC.SuppressFinalize(this);
    }
}