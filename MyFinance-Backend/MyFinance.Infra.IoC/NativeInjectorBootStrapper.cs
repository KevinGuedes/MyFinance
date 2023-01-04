using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MyFinance.Application.BusinessUnits.ApiService;
using MyFinance.Application.MonthlyBalances.ApiService;
using MyFinance.Application.Pipelines;
using MyFinance.Application.Transfers.ApiService;
using MyFinance.Domain.Interfaces;
using MyFinance.Infra.Data.Context;
using MyFinance.Infra.Data.Mappers;
using MyFinance.Infra.Data.Repositories;
using MyFinance.Infra.Data.Settings;
using MyFinance.Infra.Data.UnitOfWork;
using System.Reflection;

namespace MyFinance.Infra.IoC
{
    public static class NativeInjectorBootStrapper
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            RegisterData(services, configuration);
            RegisterApiServices(services);

            var applicationLayerAssembly = Assembly.Load("MyFinance.Application");
            RegisterAutoMapper(services, applicationLayerAssembly);
            RegisterValidators(services, applicationLayerAssembly);
            RegisterMediatR(services, applicationLayerAssembly);
        }

        private static void RegisterApiServices(IServiceCollection services)
            => services
                .AddScoped<IBusinessUnitApiService, BusinessUnitApiService>()
                .AddScoped<IMonthlyBalanceApiService, MonthlyBalanceApiService>()
                .AddScoped<ITransferApiService, TransferApiService>();

        private static void RegisterData(IServiceCollection services, IConfiguration configuration)
        {
            MongoDbMapper.MapEntities();

            services.Configure<MongoSettings>(configuration.GetSection(nameof(MongoSettings)));
            var mongoSettings = services.BuildServiceProvider().GetService<IOptions<MongoSettings>>()!.Value;

            services
                .AddScoped<IMongoClient, MongoClient>(sp => new MongoClient(mongoSettings.ConnectionString))
                .AddScoped<IMongoContext, MongoContext>()
                .AddScoped<IUnitOfWork, UnitOfWork>();

            services
                .AddScoped<IBusinessUnitRepository, BusinessUnitRepository>()
                .AddScoped<IMonthlyBalanceRepository, MonthlyBalanceRepository>();
        }

        public static void RegisterAutoMapper(IServiceCollection services, Assembly applicationLayerAssembly)
        {
            services.AddFluentValidationClientsideAdapters();
            services.AddValidatorsFromAssembly(applicationLayerAssembly);
        }

        private static void RegisterValidators(IServiceCollection services, Assembly applicationLayerAssembly)
            => services.AddAutoMapper(applicationLayerAssembly);

        private static void RegisterMediatR(IServiceCollection services, Assembly applicationLayerAssembly)
           => services
                .AddMediatR(applicationLayerAssembly)
                .AddScoped(typeof(IPipelineBehavior<,>), typeof(ExceptionHandlerPipeline<,>))
                .AddScoped(typeof(IPipelineBehavior<,>), typeof(RequestValidationPipeline<,>))
                .AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingPipeline<,>))
                .AddScoped(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkPipeline<,>));
    }
}
