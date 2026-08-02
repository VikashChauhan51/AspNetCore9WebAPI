using CourseLibrary.Logging.Telemetry.Configurations;
using CourseLibrary.Logging.Telemetry.Enrichments;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Trace;

namespace CourseLibrary.Logging.Telemetry.Extensions;

public static class OpenTelemetryCloudEnrichment
{
    public static TracerProviderBuilder AddCloudEnrichment(this TracerProviderBuilder builder, string provider, IServiceProvider serviceProvider)
    {
        var config = serviceProvider.GetRequiredService<IConfiguration>();
        var processor = new CloudEnrichmentProcessor(config);
        return builder.AddProcessor(processor);
    }

    public static IServiceCollection TelemetryConfigure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<OtlpExporterOptions>(
       configuration.GetSection("OtlpExporter"));
        return services;
    }
}