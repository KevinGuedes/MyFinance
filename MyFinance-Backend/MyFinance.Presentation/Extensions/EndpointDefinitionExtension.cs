using Microsoft.Extensions.DependencyInjection.Extensions;
using MyFinance.Presentation.Interfaces;
using System.Reflection;

namespace MyFinance.Presentation.Extensions;

public static class EndpointDefinitionExtension
{
    public static IServiceCollection AddEndpointGroupDefinitions(this IServiceCollection services, Assembly assembly)
    {
        var serviceDescriptors = assembly
            .DefinedTypes
            .Where(type => 
                type is { IsAbstract: false, IsInterface: false } &&
                type.IsAssignableTo(typeof(IEndpointGroupDefinition)))
            .Select(type =>
                ServiceDescriptor.Transient(typeof(IEndpointGroupDefinition), type))
            .ToArray();

        services.TryAddEnumerable(serviceDescriptors);

        return services;
    }

    public static IApplicationBuilder MapEndpointGroups(this WebApplication app)
    {
        var endpointGroups = app.Services.GetRequiredService<IEnumerable<IEndpointGroupDefinition>>();
        var builder = app.MapGroup("").RequireAuthorization();

        foreach (var endpointGroup in endpointGroups)
            endpointGroup.MapEndpoint(builder);

        return app;
    }
}
