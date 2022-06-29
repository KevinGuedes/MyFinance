using MyFinance.Infra.IoC;
using MyFinance.Presentation.Configurations;
using MyFinance.Presentation.Filters;
using MyFinance.Presentation.Middlewares;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services
        .AddControllers(options => options.Filters.Add<ModelStateValidationFilter>())
        .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddCustomSwaggerConfiguration();
    builder.Services.RegisterServices(builder.Configuration);
}

var app = builder.Build();
{
    if (app.Environment.IsDevelopment())
        app.UseDeveloperExceptionPage();

    app.UseCustomSwaggerConfiguration();
    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();
    app.UseMiddleware<ExceptionHandlerMiddleware>();
    app.Run();
}
