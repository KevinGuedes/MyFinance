using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyFinance.Application.Abstractions.Services;
using MyFinance.Infrastructure.Services.CurrentUserProvider;

namespace MyFinance.Infrastructure.Persistence.Context;

internal class DesignTimeMyFinanceDbContextFactory : IDesignTimeDbContextFactory<MyFinanceDbContext>
{
    public MyFinanceDbContext CreateDbContext(string[] args)
    {
        var path = Directory.GetCurrentDirectory();
        var configurationBuilder = new ConfigurationBuilder();
        configurationBuilder.SetBasePath(path).AddJsonFile("appsettings.json");

        var configuration = configurationBuilder.Build();
        var connectionString = configuration.GetConnectionString("MyFinanceDb");

        var optionsBuilder = new DbContextOptionsBuilder<MyFinanceDbContext>();
        optionsBuilder.UseSqlServer(connectionString);

        var services = new ServiceCollection();
        services
            .AddHttpContextAccessor()
            .AddTransient<ICurrentUserProvider, CurrentUserProvider>();

        var serviceProvider = services.BuildServiceProvider();
        var currentUserProvider = serviceProvider.GetRequiredService<ICurrentUserProvider>();

        return new MyFinanceDbContext(optionsBuilder.Options, currentUserProvider);
    }
}
