using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyFinance.Infrastructure.Persistence.Context;
using MyFinance.TestCommon.Builders.Users;
using System.Text.Encodings.Web;
using Testcontainers.MsSql;

namespace MyFinance.IntegrationTests.Common;

public sealed class ApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly MsSqlContainer _dbContainer = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
        .WithName("sqlserver-my-finance-integration-tests")
        .Build();

    public Task InitializeAsync() => _dbContainer.StartAsync();

    Task IAsyncLifetime.DisposeAsync() => _dbContainer.StopAsync();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((context, config) =>
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                { "EmailSenderOptions:UseEmailConfirmation", "false" }
            })
        );

        builder.ConfigureTestServices(services =>
        {
            var dbContextDescriptior = services
                .SingleOrDefault(service => service.ServiceType == typeof(DbContextOptions<MyFinanceDbContext>));

            if (dbContextDescriptior is not null)
                services.Remove(dbContextDescriptior);

            services.AddDbContext<MyFinanceDbContext>(options =>
                options.UseSqlServer(_dbContainer.GetConnectionString()));

            using var scope = services.BuildServiceProvider().CreateScope();
            var myFinanceDbContext = scope.ServiceProvider.GetRequiredService<MyFinanceDbContext>();

            EnsureDatabaseCreation(myFinanceDbContext);
            SeedDatabase(myFinanceDbContext);
            
            SetupTestAuthentication(services);
        });
    }

    private static void EnsureDatabaseCreation(MyFinanceDbContext myFinanceDbContext)
    {
        myFinanceDbContext.Database.Migrate();
        myFinanceDbContext.Database.EnsureCreated();
    }

    private static void SeedDatabase(MyFinanceDbContext myFinanceDbContext)
    {
        myFinanceDbContext.Users.Add(UserDirector.CreateDefaultTestUser().User);
        myFinanceDbContext.Users.Add(UserDirector.CreateUserWithOldPassword().User);
        myFinanceDbContext.SaveChanges();
    }

    private static void SetupTestAuthentication(IServiceCollection services)
    {
        services
               .AddAuthentication(TestAuthenticationHandler.TestSchemeName)
               .AddScheme<AuthenticationSchemeOptions, TestAuthenticationHandler>(
                   TestAuthenticationHandler.TestSchemeName,
                   options => { }
               );

        var authHandlerDescriptor = services
            .First(service => service.ImplementationType == typeof(TestAuthenticationHandler));

        if (authHandlerDescriptor is not null)
            services.Remove(authHandlerDescriptor);

        services.AddTransient(implementationFactory =>
        {
            var optionsMonitor = implementationFactory.GetRequiredService<IOptionsMonitor<AuthenticationSchemeOptions>>();
            var loggerFactory = implementationFactory.GetRequiredService<ILoggerFactory>();
            var urlEncoder = implementationFactory.GetRequiredService<UrlEncoder>();

            return new TestAuthenticationHandler(
                optionsMonitor,
                loggerFactory,
                urlEncoder,
                UserDirector.CreateDefaultTestUser().User.Id);
        });
    }
}
