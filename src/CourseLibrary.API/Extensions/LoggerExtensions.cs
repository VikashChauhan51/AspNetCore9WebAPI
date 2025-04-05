using CourseLibrary.Logging.Loggers.Configurations;
using CourseLibrary.Logging.Loggers.Enrichments;
using Serilog.Core;
using Serilog;
using Microsoft.Extensions.Options;
using CourseLibrary.Logging.Telemetry.Configurations;

namespace CourseLibrary.API.Extensions;

public static class LoggerExtensions
{
    public static void ConfigureLogger(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<ApplicationOptions>(
        builder.Configuration.GetSection("Application"));

        builder.Services.AddSingleton<ILogEventEnricher, HttpContextEnricher>();
        builder.Host.UseSerilog((context, services, config) =>
        {
            var enrichers = services.GetServices<ILogEventEnricher>().ToArray();
            var application = services.GetRequiredService<IOptions<ApplicationOptions>>().Value;
            var OtlpEndpoint = services
           .GetRequiredService<IOptions<OtlpExporterOptions>>()
           .Value;

            config
                .ReadFrom.Configuration(context.Configuration)
                .Enrich.FromLogContext()
                .Enrich.With(enrichers)
                .Enrich.WithEnvironmentName()
                .Enrich.WithMachineName()
                .Enrich.WithProperty("ApplicationName", application?.Name ?? "CourseLibrary")
                .Enrich.WithProperty("ApplicationVersion", application?.Version)
                .Enrich.WithThreadName()
                .Enrich.WithThreadId()
                .Enrich.WithProcessId()
                .Enrich.WithProcessName()
                .WriteTo.Console();

            // Exports logs to OpenTelemetry.
            if (OtlpEndpoint != null && OtlpEndpoint.Endpoint != null)
            {
                config.WriteTo.OpenTelemetry(OtlpEndpoint.Endpoint.OriginalString);
            }
        });
    }
}
