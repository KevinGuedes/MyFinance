using Microsoft.Extensions.DependencyInjection;
using MyFinance.Infrastructure.Persistence.Context;
using MyFinance.IntegrationTests.SharedContexts;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MyFinance.IntegrationTests.Common;

[Collection(nameof(ApplicationFactoryCollection))]
public abstract class BaseIntegrationTest : IDisposable
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

        var responseDataAsJson = await response.Content.ReadAsStringAsync();
        var responseData = JsonSerializer.Deserialize<TResponse>(responseDataAsJson, _jsonSerializerOptions);

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

    protected static async Task VerifyResponseAsync(
        HttpResponseMessage response,
        HttpStatusCode expectedStatusCode)
    {
        Assert.Equal(expectedStatusCode, response.StatusCode);

        if (response.Content.Headers.ContentLength is 0)
            return;

        var responseContent = await response.Content.ReadAsStringAsync();

        DerivePathInfo((_, projectDirectory, type, method) =>
        {
            return new PathInfo(
                Path.Combine(projectDirectory, "ResponseSnapshots", type.Name),
                type.Name,
                method.Name);
        });

        await VerifyJson(responseContent).UseStrictJson();
    }

    public void Dispose()
    {
        _scope?.Dispose();
        _myFinanceDbContext?.Dispose();
        GC.SuppressFinalize(this);
    }
}