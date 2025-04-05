using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using OpenTelemetry;
using CourseLibrary.Logging.Telemetry.Filters;
using CourseLibrary.Logging.Telemetry.Extensions;
using Microsoft.Extensions.Options;
using CourseLibrary.Logging.Telemetry.Configurations;
using Microsoft.Identity.Client;

namespace CourseLibrary.API.Extensions;

public static class OpenTelemetryExtensions
{

    public static void ConfigureOpenTelemetry(this WebApplicationBuilder builder)
    {
        builder.Services.TelemetryConfigure(builder.Configuration);
        // Setup logging to be exported via OpenTelemetry
        builder.Logging.AddOpenTelemetry(logging =>
        {
            logging.IncludeFormattedMessage = true;
            logging.IncludeScopes = true;
        });

        var otel = builder.Services.AddOpenTelemetry();
        var OtlpEndpoint = builder.Services
           .BuildServiceProvider()
           .GetRequiredService<IOptions<OtlpExporterOptions>>()
           .Value;

        // Add Metrics for ASP.NET Core and our custom metrics and export via OTLP
        otel.WithMetrics(metrics =>
        {
            // Metrics provider from OpenTelemetry
            metrics.AddAspNetCoreInstrumentation();
            metrics.AddSqlClientInstrumentation();
            metrics.AddHttpClientInstrumentation();
            metrics.AddRuntimeInstrumentation();
            // Metrics provides by ASP.NET Core in .NET 8
            metrics.AddMeter("Microsoft.AspNetCore.Hosting");
            metrics.AddMeter("Microsoft.AspNetCore.Server.Kestrel");

            //Export Metrics data
            if (OtlpEndpoint != null && OtlpEndpoint.Endpoint != null)
            {
                metrics.AddOtlpExporter(options =>
                {
                    options.Endpoint = OtlpEndpoint.Endpoint;
                    options.Protocol = OtlpEndpoint.Protocol;
                });
            }
        });

        // Add Tracing for ASP.NET Core and our custom ActivitySource and export via OTLP
        otel.WithTracing(tracing =>
        {
            tracing.AddAspNetCoreInstrumentationFilterForSwaggerAndHealth();
            tracing.AddHttpClientInstrumentation();
            tracing.AddSqlClientInstrumentation();
            tracing.AddEntityFrameworkCoreInstrumentation();
            tracing.AddHttpClientInstrumentation();

            //Export Tracing data
            if (OtlpEndpoint != null && OtlpEndpoint.Endpoint != null)
            {
                tracing.AddOtlpExporter(options =>
                {
                    options.Endpoint = OtlpEndpoint.Endpoint;
                    options.Protocol = OtlpEndpoint.Protocol;
                });
            }
        });
    }
}
