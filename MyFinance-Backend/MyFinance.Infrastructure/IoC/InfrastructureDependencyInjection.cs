using Microsoft.AspNetCore.Authentication.Cookies;
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
using MyFinance.Infrastructure.Services.Summary;
using System.Reflection;

namespace MyFinance.Infrastructure.IoC;

public static class InfrastructureDependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        => services
            .AddHttpContextAccessor()
            .AddAuth()
            .AddInfrastructureServices()
            .AddPersistence(configuration)
            .AddHelthCheckForExternalServices();

    private static IServiceCollection AddAuth(this IServiceCollection services)
    {
        services
            .ConfigureOptions<CookieConfiguration>()
            .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie();

        return services.AddAuthorization();
    }

    private static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        => services
            .AddScoped<ISummaryService, SummaryService>()
            .AddScoped<IPasswordHasher, PasswordHasher>()
            .AddScoped<IAuthService, AuthService>()
            .AddScoped<ICurrentUserProvider, CurrentUserProvider>();

    private static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<MyFinanceDbContext>(
            options => options
                .UseSqlServer(configuration.GetConnectionString("MyFinanceDb"),
                    sqlServerOptions => sqlServerOptions
                        .UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery)
                        .MigrationsAssembly(Assembly.GetExecutingAssembly().FullName)));

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<IBusinessUnitRepository, BusinessUnitRepository>()
            .AddScoped<ITransferRepository, TransferRepository>()
            .AddScoped<IAccountTagRepository, AccountTagRepository>()
            .AddScoped<ICategoryRepository, CategoryRepository>();
    }

    private static IServiceCollection AddHelthCheckForExternalServices(this IServiceCollection services)
    {
        services
            .AddHealthChecks()
            .AddDbContextCheck<MyFinanceDbContext>("database");

        return services;
    }
}