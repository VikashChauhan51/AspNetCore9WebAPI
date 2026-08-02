using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CourseLibrary.Models.Extensions;

public static class MapsterServiceExtensions
{
    public static IServiceCollection AddMapsterConfiguration(this IServiceCollection services, params Assembly[] assemblies)
    {
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(assemblies);
        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();

        return services;
    }
}
