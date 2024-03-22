using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using MyFinance.Application.RequestPipeline.Behaviors;
using MyFinance.Application.RequestPipeline.PostProcessors;
using System.Reflection;

namespace MyFinance.Application.IoC;

public static class ApplicationDependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
        => services
            .AddValidators()
            .AddMediatR();

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
                    .AddOpenBehavior(typeof(TimingBehavior<,>))
                    .AddOpenBehavior(typeof(UserProviderBehavior<,>))
                    .AddOpenBehavior(typeof(ValidationBehavior<,>))
                    .AddOpenRequestPostProcessor(typeof(UnitOfWorkPostProcessor<,>));
            });
}