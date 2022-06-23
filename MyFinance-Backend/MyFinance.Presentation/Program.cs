using MyFinance.Infra.IoC;
using MyFinance.Presentation.Filters;
using MyFinance.Presentation.Middlewares;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddControllers(options => options.Filters.Add<ModelStateValidationFilter>());
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.RegisterServices(builder.Configuration);
}

var app = builder.Build();
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();
    app.UseMiddleware<ExceptionHandlerMiddleware>();
    app.Run();
}
