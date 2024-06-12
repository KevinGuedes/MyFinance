using MyFinance.Application.IoC;
using MyFinance.Infrastructure.IoC;
using MyFinance.Presentation.Endpoints;
using MyFinance.Presentation.IoC;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services
        .AddInfrastructure(builder.Configuration)
        .AddApplication()
        .AddPresentation()
        .AddMvcCore(); //required for some services to be injected
}

var app = builder.Build();
{
    app.UseExceptionHandler();

    if (app.Environment.IsDevelopment())
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
    //app.MapControllers().RequireAuthorization();
    app
        .MapGroup("")
        .RequireAuthorization()
        .AddUserEndpoints(); //approach using extension methods

    app.Run();
}