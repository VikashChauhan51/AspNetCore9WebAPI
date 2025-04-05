using CourseLibrary.Logging.Loggers.Enrichments;
using Serilog.Core;
using Serilog;
using Microsoft.Extensions.Caching.Hybrid;
using CourseLibrary.Caching;
using Microsoft.Extensions.Options;

namespace CourseLibrary.API.Extensions;

public static class CachingExtensions
{
    public static void ConfigureCache(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<HybridCacheEntryOptions>(
        builder.Configuration.GetSection("Caching"));
        var cacheEntryOptions = builder.Services
            .BuildServiceProvider()
            .GetRequiredService<IOptions<HybridCacheEntryOptions>>()
            .Value;

        builder.Services.AddHybridCache(options =>
        {
            options.MaximumPayloadBytes = 1024 * 1024;
            options.MaximumKeyLength = 1024;
            options.DefaultEntryOptions = new HybridCacheEntryOptions
            {
                Expiration = cacheEntryOptions?.Expiration ?? TimeSpan.FromMinutes(5),
                LocalCacheExpiration = cacheEntryOptions?.LocalCacheExpiration ?? TimeSpan.FromMinutes(5)
            };
        });
        builder.Services.AddSingleton<ICacheProvider, HybridCacheProvider>();
        builder.Services.AddSingleton<IEntitySerializer, JsonEntitySerializer>();
    }
}
