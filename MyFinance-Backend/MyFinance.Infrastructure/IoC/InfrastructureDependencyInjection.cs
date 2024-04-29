using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Application.Abstractions.Persistence.UnitOfWork;
using MyFinance.Application.Abstractions.Services;
using MyFinance.Infrastructure.Abstractions;
using MyFinance.Infrastructure.Persistence.Context;
using MyFinance.Infrastructure.Persistence.Repositories;
using MyFinance.Infrastructure.Persistence.UnitOfWork;
using MyFinance.Infrastructure.Services.CurrentUserProvider;
using MyFinance.Infrastructure.Services.EmailSender;
using MyFinance.Infrastructure.Services.LockoutManager;
using MyFinance.Infrastructure.Services.PasswordManager;
using MyFinance.Infrastructure.Services.SignInManager;
using MyFinance.Infrastructure.Services.Summary;
using MyFinance.Infrastructure.Services.TokenProvider;
using System.Reflection;

namespace MyFinance.Infrastructure.IoC;

public static class InfrastructureDependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddDataProtection();

        return services
            .AddHttpContextAccessor()
            .AddAuth()
            .AddPersistence(configuration)
            .AddHelthCheckForExternalServices()
            .AddInfrastructureServices();
    }

    private static IServiceCollection AddAuth(this IServiceCollection services)
    {
        services
            .ConfigureOptions<CookieConfiguration>()
            .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie();

        return services.AddAuthorization();
    }

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

    private static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services
            .AddOptionsWithValidationOnStart<TokenOptions>()
            .AddOptionsWithValidationOnStart<LockoutOptions>()
            .AddOptionsWithValidationOnStart<PasswordOptions>();

        return services
            .AddScoped<ISummaryService, SummaryService>()
            .AddScoped<ITokenProvider, TokenProvider>()
            .AddScoped<IPasswordManager, PasswordManager>()
            .AddScoped<ILockoutManager, LockoutManager>()
            .AddScoped<ISignInManager, SignInManager>()
            .AddScoped<ICurrentUserProvider, CurrentUserProvider>()
            .AddScoped<IEmailSender, EmailSender>();
    }

    private static IServiceCollection AddOptionsWithValidationOnStart<TOptions>(
        this IServiceCollection services,
        Action<TOptions>? configureOptions = null)
        where TOptions : class, IValidatableOptions
    {
        var optionsBuilder = services
            .AddOptions<TOptions>()
            .ValidateOnStart()
            .ValidateDataAnnotations();

        if(configureOptions is not null)
            optionsBuilder.Configure(configureOptions);

        return services;
    }
}