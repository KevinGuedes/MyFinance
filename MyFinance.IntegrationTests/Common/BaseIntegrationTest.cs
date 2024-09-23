using Microsoft.Extensions.DependencyInjection;
using MyFinance.Infrastructure.Persistence.Context;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MyFinance.IntegrationTests.Common;

public abstract class BaseIntegrationTest : IClassFixture<ApplicationFactory>, IDisposable
{
    private readonly string _baseUrl;
    private readonly IServiceScope _scope;
    private readonly JsonSerializerOptions _jsonSerializerOptions;
    private readonly HttpClient _httpClient;
    private readonly MyFinanceDbContext _myFinanceDbContext;

    protected BaseIntegrationTest(ApplicationFactory applicationFactory, string baseUrl)
    {
        _baseUrl = baseUrl;
        _jsonSerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
        };

        _httpClient = applicationFactory.CreateClient();
        _httpClient.DefaultRequestHeaders.Authorization = new(TestAuthenticationHandler.TestSchemeName);

        _scope = applicationFactory.Services.CreateScope();
        _myFinanceDbContext = _scope.ServiceProvider.GetRequiredService<MyFinanceDbContext>();
    }

    protected async Task<(HttpResponseMessage, TResponse?)> PostAsync<TRequest, TResponse>(
        TRequest request,
        string? requestUrl = null)
    {
        var endpoint = requestUrl is null ? _baseUrl : $"{_baseUrl}/{requestUrl}";
        var response = await _httpClient.PostAsJsonAsync(endpoint, request);

        var responseDataAsString = await response.Content.ReadAsStringAsync();
        var responseData = JsonSerializer.Deserialize<TResponse>(responseDataAsString, _jsonSerializerOptions);
        
        return (response, responseData);
    }

    protected async Task<HttpResponseMessage> PostAsync<TRequest>(
        TRequest request,
        string? requestUrl = null)
    {
        var endpoint = requestUrl is null ? _baseUrl : $"{_baseUrl}/{requestUrl}";
        var response = await _httpClient.PostAsJsonAsync(endpoint, request);

        return response;
    }

    public void Dispose()
    {
        _scope?.Dispose();
        _myFinanceDbContext?.Dispose();
        GC.SuppressFinalize(this);
    }
}