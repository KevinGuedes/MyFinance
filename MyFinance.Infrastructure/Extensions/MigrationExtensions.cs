using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyFinance.Infrastructure.Persistence.Context;

namespace MyFinance.Infrastructure.Extensions;

public static class MigrationExtensions
{
    public static void ApplyDatabaseMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<MyFinanceDbContext>();
        dbContext.Database.Migrate();
    }
}
