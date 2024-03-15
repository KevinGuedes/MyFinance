using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Application.Abstractions.Persistence.UnitOfWork;
using MyFinance.Application.Abstractions.Services;
using MyFinance.Infrastructure.Persistence.Context;
using MyFinance.Infrastructure.Persistence.Repositories;
using MyFinance.Infrastructure.Persistence.UnitOfWork;
using MyFinance.Infrastructure.Services.Auth;
using MyFinance.Infrastructure.Services.CurrentUserProvider;
using MyFinance.Infrastructure.Services.PasswordHasher;
using MyFinance.Infrastructure.Services.Spreadsheet;

namespace MyFinance.Infrastructure.IoC;

public static class InfrastructureDependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        => services.AddAuth()
            .AddServices()
            .AddPersistence(configuration);

    private static IServiceCollection AddAuth(this IServiceCollection services)
    {
        services
            .AddHttpContextAccessor()
            .AddAuthentication()
            .AddCookie(options =>
            {
                options.Cookie.Name = "MyFinance.Cookie";
                options.Cookie.HttpOnly = true;
                options.Cookie.SameSite = SameSiteMode.Strict;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.SlidingExpiration = true;
                options.Cookie.IsEssential = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
            });

        return services.AddAuthorization();
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
        => services
            .AddScoped<ISpreadsheetService, SpreadsheetService>()
            .AddScoped<IPasswordHasher, PasswordHasher>()
            .AddScoped<IAuthService, AuthService>()
            .AddScoped<ICurrentUserProvider, CurrentUserProvider>();

    private static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var migrationsAssemblyName = typeof(MyFinanceDbContext).Assembly.FullName;
        services.AddDbContext<MyFinanceDbContext>(
            options => options
                .UseSqlite(configuration.GetConnectionString("Lite"),
                    sqlServerOptions => sqlServerOptions
                        .UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery)
                        .MigrationsAssembly(migrationsAssemblyName)));

        return services
            .AddScoped<IUnitOfWork, UnitOfWork>()
            .AddScoped<IBusinessUnitRepository, BusinessUnitRepository>()
            .AddScoped<IMonthlyBalanceRepository, MonthlyBalanceRepository>()
            .AddScoped<ITransferRepository, TransferRepository>()
            .AddScoped<IAccountTagRepository, AccountTagRepository>()
            .AddScoped<IUserRepository, UserRepository>();
    }
}