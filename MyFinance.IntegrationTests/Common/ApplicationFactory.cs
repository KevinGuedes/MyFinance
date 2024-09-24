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

            if (dbContextDescriptior is not null) services.Remove(dbContextDescriptior);

            services.AddDbContext<MyFinanceDbContext>(options =>
                options.UseSqlServer(_dbContainer.GetConnectionString()));

            var sp = services.BuildServiceProvider();

            using var scope = sp.CreateScope();
            var myFinanceDbContext = scope.ServiceProvider.GetRequiredService<MyFinanceDbContext>();
            myFinanceDbContext.Database.Migrate();
            myFinanceDbContext.Database.EnsureCreated();
            var defaultTestUserId = SeedUsersData(myFinanceDbContext);

            services
               .AddAuthentication(TestAuthenticationHandler.TestSchemeName)
               .AddScheme<AuthenticationSchemeOptions, TestAuthenticationHandler>(
                   TestAuthenticationHandler.TestSchemeName,
                   options => { }
               );

            var authHandlerDescriptor = services
                .First(service => service.ImplementationType == typeof(TestAuthenticationHandler));

            if (authHandlerDescriptor is not null) services.Remove(authHandlerDescriptor);

            services.AddTransient(implementationFactory =>
            {
                var optionsMonitor = implementationFactory.GetRequiredService<IOptionsMonitor<AuthenticationSchemeOptions>>();
                var loggerFactory = implementationFactory.GetRequiredService<ILoggerFactory>();
                var urlEncoder = implementationFactory.GetRequiredService<UrlEncoder>();

                return new TestAuthenticationHandler(
                    optionsMonitor,
                    loggerFactory,
                    urlEncoder,
                    defaultTestUserId);
            });
        });
    }

    private static Guid SeedUsersData(MyFinanceDbContext myFinanceDbContext)
    {
        myFinanceDbContext.Users.RemoveRange(myFinanceDbContext.Users);
        
        var user = myFinanceDbContext.Users.Add(UserFactory.DefaultTestUser);

        var userWithOldPassword = UserFactory.UserWithOldPassword;
        var lastPasswordUpdate = DateTime.UtcNow.AddMonths(-6).AddSeconds(-1);
        myFinanceDbContext.Database.ExecuteSql($@"INSERT INTO 
            Users (Id, Name, Email, PasswordHash, FailedSignInAttempts, IsEmailVerified, LockoutEndOnUtc, LastPasswordUpdateOnUtc, SecurityStamp, CreatedOnUtc, UpdatedOnUtc)
            VALUES ({Guid.NewGuid()}, {userWithOldPassword.Name}, {userWithOldPassword.Email}, {userWithOldPassword.PasswordHash}, 0, 1, NULL, {lastPasswordUpdate}, {userWithOldPassword.SecurityStamp}, {DateTime.UtcNow}, NULL)");
        
        myFinanceDbContext.SaveChanges();
        return user.Entity.Id;
    } 
}
