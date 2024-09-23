using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyFinance.Application.Abstractions.Services;
using MyFinance.Infrastructure.Persistence.Context;
using MyFinance.Infrastructure.Services.CurrentUserProvider;
using MyFinance.IntegrationTests.MockServices;
using MyFinance.TestCommon.Factories;
using System.Text.Encodings.Web;
using Testcontainers.MsSql;

namespace MyFinance.IntegrationTests.Common;

public class ApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly MsSqlContainer _dbContainer = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
        .WithPassword("Strong_password_123!")
        .Build();

    public Task InitializeAsync() => _dbContainer.StartAsync();

    Task IAsyncLifetime.DisposeAsync() => _dbContainer.StopAsync();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            var dbContextDescriptior = services
                .SingleOrDefault(service => service.ServiceType == typeof(DbContextOptions<MyFinanceDbContext>));
            var currentUserProviderDescriptor = services
                .SingleOrDefault(service => service.ServiceType == typeof(CurrentUserProvider));

            if (dbContextDescriptior is not null) services.Remove(dbContextDescriptior);
            if (currentUserProviderDescriptor is not null) services.Remove(currentUserProviderDescriptor);

            services.AddDbContext<MyFinanceDbContext>(options =>
                options.UseSqlServer(_dbContainer.GetConnectionString()));

            var sp = services.BuildServiceProvider();

            using var scope = sp.CreateScope();
            var myFinanceDbContext = scope.ServiceProvider.GetRequiredService<MyFinanceDbContext>();
            myFinanceDbContext.Database.Migrate();
            myFinanceDbContext.Database.EnsureCreated();

            var userId = SeedTestUser(myFinanceDbContext);

            services.AddScoped<ICurrentUserProvider>(sp =>
            {
                return new MockCurrentUserProvider(userId);
            });

            services
               .AddAuthentication(TestAuthenticationHandler.TestSchemeName)
               .AddScheme<AuthenticationSchemeOptions, TestAuthenticationHandler>(
                   TestAuthenticationHandler.TestSchemeName,
                   options => { }
               );

            var authHandlerDescriptor = services
                .First(s => s.ImplementationType == typeof(TestAuthenticationHandler));

            if (authHandlerDescriptor is not null) services.Remove(authHandlerDescriptor);

            services.AddTransient(sp =>
            {
                var optionsMonitos = sp.GetRequiredService<IOptionsMonitor<AuthenticationSchemeOptions>>();
                var loggerFactory = sp.GetRequiredService<ILoggerFactory>();
                var urlEncoder = sp.GetRequiredService<UrlEncoder>();

                return new TestAuthenticationHandler(optionsMonitos, loggerFactory, urlEncoder, userId);
            });
        });
    }

    private static Guid SeedTestUser(MyFinanceDbContext myFinanceDbContext)
    {
        myFinanceDbContext.Users.RemoveRange(myFinanceDbContext.Users);
        var user = myFinanceDbContext.Users.Add(UserFactory.CreateUser());
        myFinanceDbContext.SaveChanges();

        return user.Entity.Id;
    } 
}
