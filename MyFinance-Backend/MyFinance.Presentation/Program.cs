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

    if (!app.Environment.IsProduction())
    {
        app
            .UseSwagger()
            .UseSwaggerUI(options =>
            {
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