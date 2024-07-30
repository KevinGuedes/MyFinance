using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyFinance.Application.Abstractions.Persistence;
using MyFinance.Application.Abstractions.Services;
using MyFinance.Infrastructure.Abstractions;
using MyFinance.Infrastructure.Persistence.Context;
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
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddDataProtection();

        return services
            .AddHttpContextAccessor()
            .AddAuth()
            .AddPersistence(configuration)
            .AddHelthCheckForExternalServices()
            .AddInfrastructureServices(configuration);
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
                .UseSqlServer(configuration.GetConnectionString("Database"),
                    sqlServerOptions => sqlServerOptions
                        .UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery)
                        .MigrationsAssembly(Assembly.GetExecutingAssembly().FullName)
                        .EnableRetryOnFailure(10, TimeSpan.FromSeconds(15), [])));

        return services
             .AddScoped<IMyFinanceDbContext, MyFinanceDbContext>()
             .AddScoped<IUnitOfWork, UnitOfWork>();
    }

    private static IServiceCollection AddHelthCheckForExternalServices(this IServiceCollection services)
    {
        services
            .AddHealthChecks()
            .AddDbContextCheck<MyFinanceDbContext>("database");

        return services;
    }

    private static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddOptionsWithValidationOnStart<LockoutOptions>();

        services
            .BindOptionsWithValidationOnStart<TokenOptions>(configuration)
            .BindOptionsWithValidationOnStart<PasswordOptions>(configuration);

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

        if (configureOptions is not null)
            optionsBuilder.Configure(configureOptions);

        return services;
    }

    private static IServiceCollection BindOptionsWithValidationOnStart<TOptions>(
        this IServiceCollection services,
        IConfiguration configuration)
        where TOptions : class, IValidatableOptions
    {
        services
            .AddOptions<TOptions>()
            .Bind(configuration.GetSection(typeof(TOptions).Name))
            .ValidateOnStart()
            .ValidateDataAnnotations();

        return services;
    }
}