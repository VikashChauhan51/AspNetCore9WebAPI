using FastEndpoints;
using Microsoft.Extensions.Caching.Hybrid;

namespace CourseLibrary.API.Extensions;
public static class ServiceExtension
{
    public static IServiceCollection CongigureServices(this IServiceCollection services, IConfiguration Configuration)
    {

        services.AddFastEndpoints()
            .AddSwaggerDocument();

        services.AddAuthorization();
        services.AddAuthentication();
        services.AddHybridCache(options =>
        {
            options.MaximumPayloadBytes = 1024 * 1024;
            options.MaximumKeyLength = 1024;
            options.DefaultEntryOptions = new HybridCacheEntryOptions
            {
                Expiration = TimeSpan.FromMinutes(5),
                LocalCacheExpiration = TimeSpan.FromMinutes(5)
            };
        });

        services.AddOptions();

        return services;
    }
}
