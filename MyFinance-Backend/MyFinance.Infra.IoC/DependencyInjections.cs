using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyFinance.Application.PipelineBehaviors;
using MyFinance.Application.UseCases.BusinessUnits.ApiService;
using MyFinance.Application.UseCases.MonthlyBalances.ApiService;
using MyFinance.Application.UseCases.Transfers.ApiService;
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
        AddPersistence(services, configuration);
        AddApiServices(services);

        var applicationLayerAssembly = Assembly.Load("MyFinance.Application");
        AddAutoMapper(services, applicationLayerAssembly);
        AddValidators(services, applicationLayerAssembly);
        AddMediatR(services, applicationLayerAssembly);
    }

    private static void AddApiServices(IServiceCollection services)
        => services
            .AddScoped<IBusinessUnitApiService, BusinessUnitApiService>()
            .AddScoped<IMonthlyBalanceApiService, MonthlyBalanceApiService>()
            .AddScoped<ITransferApiService, TransferApiService>();

    private static void AddPersistence(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<MyFinanceDbContext>(
            options => options.UseSqlServer(configuration.GetConnectionString("MyFinanceDb"),
            sqlServerOptions => sqlServerOptions
                .MigrationsAssembly(typeof(MyFinanceDbContext).Assembly.FullName)));

        services
            .AddScoped<IUnitOfWork, UnitOfWork>()
            .AddScoped<IBusinessUnitRepository, BusinessUnitRepository>()
            .AddScoped<IMonthlyBalanceRepository, MonthlyBalanceRepository>()
            .AddScoped<ITransferRepository, TransferRepository>();
    }

    public static void AddAutoMapper(IServiceCollection services, Assembly applicationLayerAssembly)
        => services
            .AddFluentValidationClientsideAdapters()
            .AddValidatorsFromAssembly(applicationLayerAssembly);

    private static void AddValidators(IServiceCollection services, Assembly applicationLayerAssembly)
       => services.AddAutoMapper(applicationLayerAssembly);

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
