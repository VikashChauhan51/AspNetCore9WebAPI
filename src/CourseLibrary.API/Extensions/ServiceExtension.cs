using CourseLibrary.Caching;

namespace CourseLibrary.API.Extensions;
public static class ServiceExtension
{
    public static IServiceCollection CongigureServices(this IServiceCollection services, IConfiguration Configuration)
    {
        services.AddSingleton<ICacheProvider, HybridCacheProvider>();      
        return services;
    }
}
