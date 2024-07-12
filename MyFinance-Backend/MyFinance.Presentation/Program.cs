using Microsoft.Extensions.Options;
using MyFinance.Application.IoC;
using MyFinance.Infrastructure.IoC;
using MyFinance.Presentation.IoC;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services
        .AddInfrastructure(builder.Configuration)
        .AddApplication()
        .AddPresentation();

    builder.Services.Configure<Nested>(builder.Configuration.GetSection(nameof(Nested)));

}

var cs = builder.Configuration.GetConnectionString("Test");

var app = builder.Build();
{
    app.UseExceptionHandler();
    app.UseCors(options =>
        options
            .AllowAnyHeader()
            .AllowAnyMethod()
            .WithOrigins(
                "https://localhost:5173", 
                "https://localhost:4173", 
                "https://victorious-water-059f4d50f.5.azurestaticapps.net/")
            .AllowCredentials());

    if (!app.Environment.IsProduction())
    {
        app
            .UseSwagger()
            .UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "MyFinance API");
                options.DocumentTitle = "MyFinance API";
                options.ConfigObject.TryItOutEnabled = true;
                options.ConfigObject.DisplayRequestDuration = true;
            });
    }

    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers().RequireAuthorization();

    app.MapGet("/cs", (IOptions<Nested> nested) =>
    {
        return new 
        { 
            CS = cs,
            Settings = app.Configuration.GetValue<string>("Value1"),
            Nested = nested.Value.Value2
        };
    });

    app.Run();
}

public class Nested
{
    public string Value2 { get; set; } = string.Empty;
}
