using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using MyFinance.Application.Abstractions.ApiServices;
using MyFinance.Application.PipelineBehaviors;
using MyFinance.Application.UseCases.AccountTags.ApiService;
using MyFinance.Application.UseCases.BusinessUnits.ApiService;
using MyFinance.Application.UseCases.MonthlyBalances.ApiService;
using MyFinance.Application.UseCases.Summary.ApiService;
using MyFinance.Application.UseCases.Transfers.ApiService;
using MyFinance.Application.UseCases.Users.ApiService;

namespace MyFinance.Application.IoC;

public static class ApplicationDependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
        => services
            .AddApiServices()
            .AddValidators()
            .AddMediatR();

    private static IServiceCollection AddApiServices(this IServiceCollection services)
        => services
            .AddScoped<IBusinessUnitApiService, BusinessUnitApiService>()
            .AddScoped<IMonthlyBalanceApiService, MonthlyBalanceApiService>()
            .AddScoped<ITransferApiService, TransferApiService>()
            .AddScoped<ISummaryApiService, SummaryApiService>()
            .AddScoped<IAccountTagApiService, AccountTagApiService>()
            .AddScoped<IUserApiService, UserApiService>();

    public static IServiceCollection AddValidators(this IServiceCollection services)
        => services
            .AddFluentValidationClientsideAdapters()
            .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

    private static IServiceCollection AddMediatR(this IServiceCollection services)
        => services
            .AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                cfg
                    .AddOpenBehavior(typeof(ExceptionHandlerBehavior<,>))
                    .AddOpenBehavior(typeof(RequestValidationBehavior<,>))
                    .AddOpenBehavior(typeof(UnitOfWorkBehavior<,>));
            });
}