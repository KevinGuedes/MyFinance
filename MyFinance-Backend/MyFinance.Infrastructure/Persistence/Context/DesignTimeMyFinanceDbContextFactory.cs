using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

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
        optionsBuilder.UseSqlite(connectionString);

        //var serviceProvider = new ServiceCollection()
        //    .AddScoped<ICurrentUserProvider, CurrentUserProvider>()
        //    .BuildServiceProvider();

        return new MyFinanceDbContext(optionsBuilder.Options, currentUserProvider: null!);
    }
}
