using MyFinance.Infra.IoC;
using MyFinance.Presentation.Configurations;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddCustomSwaggerConfiguration();
    builder.Services.AddDependencies(builder.Configuration);
}

var app = builder.Build();
{
    if (app.Environment.IsDevelopment())
        app.UseDeveloperExceptionPage();

    app.UseExceptionHandler("/error");
    app.UseCustomSwaggerConfiguration();
    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();
    app.Run();
}
