﻿using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyFinance.Application.PipelineBehaviors;
using MyFinance.Application.Services.Auth;
using MyFinance.Application.Services.CurrentUserProvider;
using MyFinance.Application.Services.PasswordHasher;
using MyFinance.Application.Services.Spreadsheet;
using MyFinance.Application.UseCases.AccountTags.ApiService;
using MyFinance.Application.UseCases.BusinessUnits.ApiService;
using MyFinance.Application.UseCases.MonthlyBalances.ApiService;
using MyFinance.Application.UseCases.Summary.ApiService;
using MyFinance.Application.UseCases.Transfers.ApiService;
using MyFinance.Application.UseCases.Users.ApiService;
using MyFinance.Domain.Interfaces;
using MyFinance.Infra.Data.Context;
using MyFinance.Infra.Data.Repositories;
using MyFinance.Infra.Data.UnitOfWork;
using System.Reflection;

namespace MyFinance.Infra.IoC;

public static class DependencyInjections
{
    public static void AddDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        AddAuth(services);
        AddApiServices(services);
        AddPersistence(services, configuration);
        AddServices(services);

        var applicationLayerAssembly = Assembly.Load("MyFinance.Application");
        AddValidators(services, applicationLayerAssembly);
        AddMediatR(services, applicationLayerAssembly);
    }

    private static void AddAuth(IServiceCollection services)
    {
        services.AddHttpContextAccessor();

        services
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

        services.AddAuthorization();
    }

    private static void AddServices(IServiceCollection services)
        => services
            .AddScoped<ISpreadsheetService, SpreadsheetService>()
            .AddScoped<IPasswordHasher, PasswordHasher>()
            .AddScoped<IAuthService, AuthService>()
            .AddScoped<ICurrentUserProvider, CurrentUserProvider>();

    private static void AddApiServices(IServiceCollection services)
        => services
            .AddScoped<IBusinessUnitApiService, BusinessUnitApiService>()
            .AddScoped<IMonthlyBalanceApiService, MonthlyBalanceApiService>()
            .AddScoped<ITransferApiService, TransferApiService>()
            .AddScoped<ISummaryApiService, SummaryApiService>()
            .AddScoped<IAccountTagApiService, AccountTagApiService>()
            .AddScoped<IUserApiService, UserApiService>();

    private static void AddPersistence(IServiceCollection services, IConfiguration configuration)
    {
        var migrationsAssemblyName = typeof(MyFinanceDbContext).Assembly.FullName;
        services.AddDbContext<MyFinanceDbContext>(
            options => options
            .UseSqlServer(configuration.GetConnectionString("MyFinanceDb"),
            sqlServerOptions => sqlServerOptions
                .UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery)
                .MigrationsAssembly(migrationsAssemblyName)));

        services
            .AddScoped<IUnitOfWork, UnitOfWork>()
            .AddScoped<IBusinessUnitRepository, BusinessUnitRepository>()
            .AddScoped<IMonthlyBalanceRepository, MonthlyBalanceRepository>()
            .AddScoped<ITransferRepository, TransferRepository>()
            .AddScoped<IAccountTagRepository, AccountTagRepository>()
            .AddScoped<IUserRepository, UserRepository>();
    }

    public static void AddValidators(IServiceCollection services, Assembly applicationLayerAssembly)
        => services
            .AddFluentValidationClientsideAdapters()
            .AddValidatorsFromAssembly(applicationLayerAssembly);

    private static void AddMediatR(IServiceCollection services, Assembly applicationLayerAssembly)
       => services
            .AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(applicationLayerAssembly);
                cfg
                    .AddOpenBehavior(typeof(ExceptionHandlerBehavior<,>))
                    .AddOpenBehavior(typeof(RequestValidationBehavior<,>))
                    .AddOpenBehavior(typeof(UnitOfWorkBehavior<,>));
            });
}
