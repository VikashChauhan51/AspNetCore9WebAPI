using CourseLibrary.Infrastructure.Observability.Metrics;
using CourseLibrary.Infrastructure.Observability.Telemetry;
using CourseLibrary.Infrastructure.Observability.Tracing;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace CourseLibrary.Api.Configuration.OpenTelemetry;

internal static class OpenTelemetryExtensions
{
    public static WebApplicationBuilder AddObservability(
        this WebApplicationBuilder builder)
    {
        var resourceBuilder = ResourceBuilder.CreateDefault();
        ConfigureResource(resourceBuilder, builder.Environment);


        builder.Logging.AddOpenTelemetry(options =>
        {
            options.SetResourceBuilder(resourceBuilder);
            options.IncludeScopes = true;
            options.IncludeFormattedMessage = true;
            options.ParseStateValues = true;
        });

        builder.Services.AddOpenTelemetry()
               .ConfigureResource(resource =>
               {
                   ConfigureResource(resource, builder.Environment);
               })
             .WithTracing(tracing =>
             {
                 tracing
                     .AddSource(ActivitySources.Default.Name)
                     .AddSource(ActivitySources.Api.Name)
                     .AddSource(ActivitySources.Application.Name)
                     .AddSource(ActivitySources.Infrastructure.Name)

                     .AddAspNetCoreInstrumentation(options =>
                     {
                         options.RecordException = true;
                     })
                     .AddHttpClientInstrumentation(options =>
                     {
                         options.RecordException = true;
                     })
                     .AddSqlClientInstrumentation(options =>
                     {
                         options.RecordException = true;
                     })
                     .AddEntityFrameworkCoreInstrumentation();
             })
             .WithMetrics(metrics =>
             {
                 metrics
                     .AddMeter(Meters.Default.Name)
                     .AddMeter(Meters.Api.Name)
                     .AddMeter(Meters.Application.Name)
                     .AddMeter(Meters.Infrastructure.Name)
                     .AddMeter(FrameworkMeters.AspNetCoreHosting)
                     .AddMeter(FrameworkMeters.Kestrel)

                     .AddAspNetCoreInstrumentation()
                     .AddHttpClientInstrumentation()
                     .AddRuntimeInstrumentation()
                     .AddSqlClientInstrumentation()
                     .AddProcessInstrumentation();
                     
             })
             .UseOtlpExporter();

        return builder;
    }

    private static void ConfigureResource(
    ResourceBuilder resource,
    IWebHostEnvironment env)
    {
        resource
            .AddService(
            serviceName: ObservabilityConstants.ServiceName,
            serviceVersion: ObservabilityConstants.ServiceVersion)
                .AddAttributes(new Dictionary<string, object>
                {
                    [Attributes.DeploymentEnvironment] = env.EnvironmentName,
                    [Attributes.ServiceInstanceId] = Environment.MachineName
                });
    }
}
