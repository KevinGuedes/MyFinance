using MyFinance.Application.IoC;
using MyFinance.Infrastructure.IoC;
using MyFinance.Presentation.Configurations;
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
    if (app.Environment.IsDevelopment())
        app.UseDeveloperExceptionPage();

    app.UseExceptionHandler("/error");
    app.UseCustomSwaggerConfiguration();
    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers().RequireAuthorization();
    app.Run();
}