using Microsoft.OpenApi.Models;

namespace MyFinance.Presentation.IoC;

public static class PresentationDependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddCustomSwaggerConfiguration();

        return services;
    }

    public static IServiceCollection AddCustomSwaggerConfiguration(this IServiceCollection services)
        => services
            .AddSwaggerGen(configuration =>
            {
                configuration.EnableAnnotations();
                configuration.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "MyFinance API",
                    Description = "API for financial management",
                    License = new OpenApiLicense { Name = "MIT" },
                    Contact = new OpenApiContact
                    {
                        Name = "Kevin Santos Gudes",
                        Email = "kevinguedes1@gmail.com",
                        Url = new Uri("https://www.linkedin.com/in/kevinsantosguedes/")
                    }
                });
            });
}