using Serilog;

namespace CourseLibrary.API.Extensions;

public static class HostBuildingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddSerilog();
        builder.ConfigureOpenTelemetry();

        builder.Services
    .CongigureServices(builder.Configuration)
    .CongigureRepositories(builder.Configuration);
        return builder.Build();
    }

    public static WebApplication ConfigurePipelines(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
        }
        app.UseHttpsRedirection();

        app.UseResponseCaching();
        app.UseRouting();
        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();

        return app;

    
    }
}
