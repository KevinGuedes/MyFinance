using MyFinance.Application.IoC;
using MyFinance.Infrastructure.IoC;
using MyFinance.Presentation.IoC;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services
        .AddInfrastructure(builder.Configuration)
        .AddApplication()
        .AddPresentation();
}

var app = builder.Build();
{
    app.UseExceptionHandler();
    app.UseCors(options =>
        options
            .AllowAnyHeader()
            .AllowAnyMethod()
            .WithOrigins(app.Configuration.GetSection("FrontendUrl").Get<string>()!)
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
    app.Run();
}