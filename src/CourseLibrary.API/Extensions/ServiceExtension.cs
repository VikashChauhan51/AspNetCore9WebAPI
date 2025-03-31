using FastEndpoints;

namespace CourseLibrary.API.Extensions;
public static class ServiceExtension
{
    public static IServiceCollection CongigureServices(this IServiceCollection services, IConfiguration Configuration)
    {

        services.AddFastEndpoints()
            .AddSwaggerDocument();
        services.AddOptions();

        return services;
    }
}
