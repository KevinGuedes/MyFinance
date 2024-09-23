using MediatR;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using MyFinance.Infrastructure.Persistence.Context;

namespace MyFinance.IntegrationTests.Common;

public abstract class BaseIntegrationTest : IClassFixture<ApplicationFactory>, IDisposable
{
    private readonly IServiceScope _scope;
    protected readonly HttpClient _httpClient;
    protected readonly ISender Sender;
    protected readonly MyFinanceDbContext DbContext;

    protected BaseIntegrationTest(ApplicationFactory factory)
    {
        _httpClient = factory.CreateClient();
        _httpClient.DefaultRequestHeaders.Authorization = new(TestAuthenticationHandler.TestSchemeName);

        _scope = factory.Services.CreateScope();
        Sender = _scope.ServiceProvider.GetRequiredService<ISender>();
        DbContext = _scope.ServiceProvider.GetRequiredService<MyFinanceDbContext>();
    }

    public void Dispose()
    {
        _scope?.Dispose();
        DbContext?.Dispose();
        GC.SuppressFinalize(this);
    }
}