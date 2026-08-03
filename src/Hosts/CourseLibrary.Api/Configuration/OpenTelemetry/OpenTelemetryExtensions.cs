using CourseLibrary.Api.Configuration.Logging.Processors;
using CourseLibrary.Api.Configuration.OpenTelemetry.Metrics;
using CourseLibrary.Api.Configuration.OpenTelemetry.Tracing;
using CourseLibrary.Api.Configuration.OpenTelemetry.Tracing.Processors;
using CourseLibrary.Application.Observability.Metrics;
using CourseLibrary.Application.Observability.Tracing;
using CourseLibrary.Infrastructure.Observability.Metrics;
using CourseLibrary.Infrastructure.Observability.Tracing;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using ApiActivitySources = CourseLibrary.Api.Configuration.OpenTelemetry.Tracing.ActivitySources;
using ApplicationActivitySources = CourseLibrary.Application.Observability.Tracing.ActivitySources;
using InfrastructureActivitySources = CourseLibrary.Infrastructure.Observability.Tracing.ActivitySources;
using ApiMeters = CourseLibrary.Api.Configuration.OpenTelemetry.Metrics.Meters;
using ApplicationMeters = CourseLibrary.Application.Observability.Metrics.Meters;
using InfrastructureMeters = CourseLibrary.Infrastructure.Observability.Metrics.Meters;

namespace CourseLibrary.Api.Configuration.OpenTelemetry;

internal static class OpenTelemetryExtensions
{
    public static WebApplicationBuilder AddObservability(
        this WebApplicationBuilder builder)
    {
        var resourceBuilder = ResourceBuilder.CreateDefault();
        ConfigureResource(resourceBuilder, builder.Environment);

        builder.Services.AddSingleton<UserActivityProcessor>();

        builder.Logging.AddOpenTelemetry(options =>
        {
            options.SetResourceBuilder(resourceBuilder);
            options.IncludeScopes = true;
            options.IncludeFormattedMessage = true;
            options.ParseStateValues = true;
            options.AddProcessor(sp => sp.GetRequiredService<CourseLibraryLogProcessor>());
        });

        builder.Services.AddOpenTelemetry()
               .ConfigureResource(resource =>
               {
                   ConfigureResource(resource, builder.Environment);
               })
             .WithTracing(tracing =>
             {
                 tracing
                     .AddSource(ApiActivitySources.Default.Name)
                     .AddSource(ApiActivitySources.Api.Name)
                     .AddSource(ApplicationActivitySources.Application.Name)
                     .AddSource(InfrastructureActivitySources.Infrastructure.Name)

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
                     .AddEntityFrameworkCoreInstrumentation()
                     .AddProcessor<UserActivityProcessor>();
             })
             .WithMetrics(metrics =>
             {
                 metrics
                     .AddMeter(ApiMeters.Default.Name)
                     .AddMeter(ApiMeters.Api.Name)
                     .AddMeter(ApplicationMeters.Application.Name)
                     .AddMeter(InfrastructureMeters.Infrastructure.Name)
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
