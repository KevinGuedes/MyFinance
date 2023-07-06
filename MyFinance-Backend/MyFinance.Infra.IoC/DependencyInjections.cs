using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyFinance.Application.BusinessUnits.ApiService;
using MyFinance.Application.MonthlyBalances.ApiService;
using MyFinance.Application.Pipelines;
using MyFinance.Application.Transfers.ApiService;
using MyFinance.Domain.Interfaces;
using MyFinance.Infra.Data.Context;
using MyFinance.Infra.Data.Repositories;
using MyFinance.Infra.Data.UnitOfWork;
using System.Reflection;

namespace MyFinance.Infra.IoC
{
    public static class DependencyInjections
    {
        public static void AddDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            AddData(services, configuration);
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

        private static void AddData(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<MyFinanceDbContext>(
                options => options.UseSqlServer(configuration.GetConnectionString("MyFinanceDb"),
                sqlServerOptions => sqlServerOptions
                    //.EnableRetryOnFailure(10, TimeSpan.FromSeconds(5), null)
                    .MigrationsAssembly(typeof(MyFinanceDbContext).Assembly.FullName)));

            services
                .AddScoped<IUnitOfWork, UnitOfWork>()
                .AddScoped<IMonthlyBalanceRepository, MonthlyBalanceRepository>()
                .AddScoped<IBusinessUnitRepository, BusinessUnitRepository>();
        }

        public static void AddAutoMapper(IServiceCollection services, Assembly applicationLayerAssembly)
        {
            services.AddFluentValidationClientsideAdapters();
            services.AddValidatorsFromAssembly(applicationLayerAssembly);
        }

        private static void AddValidators(IServiceCollection services, Assembly applicationLayerAssembly)
            => services.AddAutoMapper(applicationLayerAssembly);

        private static void AddMediatR(IServiceCollection services, Assembly applicationLayerAssembly)
           => services
                .AddMediatR(cfg =>
                {
                    cfg.RegisterServicesFromAssembly(applicationLayerAssembly);
                })
                .AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingPipeline<,>))
                .AddScoped(typeof(IPipelineBehavior<,>), typeof(ExceptionHandlerPipeline<,>))
                .AddScoped(typeof(IPipelineBehavior<,>), typeof(RequestValidationPipeline<,>))
                .AddScoped(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkPipeline<,>));
    }
}
