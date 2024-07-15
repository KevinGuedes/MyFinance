using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using MyFinance.Application.RequestPipeline.Behaviors;
using System.Reflection;

namespace MyFinance.Application.IoC;

public static class ApplicationDependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var applicationLayerAssembly = Assembly.GetExecutingAssembly();

        return services
            .AddValidators(applicationLayerAssembly)
            .AddMediatR(applicationLayerAssembly);
    }

    public static IServiceCollection AddValidators(
        this IServiceCollection services,
        Assembly applicationLayerAssembly)
        => services
            .AddFluentValidationClientsideAdapters()
            .AddValidatorsFromAssembly(applicationLayerAssembly);

    private static IServiceCollection AddMediatR(
        this IServiceCollection services,
        Assembly applicationLayerAssembly)
        => services
            .AddMediatR(cfg =>
                cfg
                    .RegisterServicesFromAssembly(applicationLayerAssembly)
                    .AddOpenBehavior(typeof(TimingBehavior<,>))
                    .AddOpenBehavior(typeof(UserProviderBehavior<,>))
                    .AddOpenBehavior(typeof(ValidationBehavior<,>))
                    .AddOpenBehavior(typeof(TransactionManagementBehavior<,>))
                    .AddOpenBehavior(typeof(PersistenceBehavior<,>)));
}