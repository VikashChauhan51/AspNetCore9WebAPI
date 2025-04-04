using CourseLibrary.Logging.Loggers.Enrichments;
using CourseLibrary.Models.Dtos;
using CourseLibrary.Models.Extensions;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.Extensions.Caching.Hybrid;
using Serilog;
using Serilog.Core;

namespace CourseLibrary.API.Extensions;

public static class HostBuildingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddOptions();
        builder.Services.AddSingleton<ILogEventEnricher, HttpContextEnricher>();      
        builder.Host.UseSerilog((context, services, config) =>
        {
            var enrichers = services.GetServices<ILogEventEnricher>().ToArray();

            config
                .ReadFrom.Configuration(context.Configuration)
                .Enrich.FromLogContext()
                .Enrich.With(enrichers)
                .Enrich.WithEnvironmentName()
                .Enrich.WithMachineName()
                .Enrich.WithProperty("ApplicationName", "CourseLibrary")
                .Enrich.WithThreadName()
                .Enrich.WithThreadId()
                .Enrich.WithProcessId()
                .Enrich.WithProcessName()
                .WriteTo.Console();
        });

        builder.ConfigureOpenTelemetry();
        builder.Services.SwaggerDocument(o =>
        {
            o.MaxEndpointVersion = 1;
            o.DocumentSettings = s =>
            {
                s.DocumentName = "Release 1";
                s.Title = "Course Library Api";
                s.Version = "v1";
            };
        });

        builder.Services.AddFastEndpoints()
            .AddSwaggerDocument();

        builder.Services.AddAuthorization();
        builder.Services.AddAuthentication();
        builder.Services.AddHybridCache(options =>
        {
            options.MaximumPayloadBytes = 1024 * 1024;
            options.MaximumKeyLength = 1024;
            options.DefaultEntryOptions = new HybridCacheEntryOptions
            {
                Expiration = TimeSpan.FromMinutes(5),
                LocalCacheExpiration = TimeSpan.FromMinutes(5)
            };
        });

        builder.Services.AddMapsterConfiguration(typeof(AuthorDto).Assembly);
        builder.Services
            .CongigureServices(builder.Configuration)
            .CongigureRepositories(builder.Configuration);

        return builder.Build();
    }

    public static WebApplication ConfigurePipelines(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
        }

        app.UseDefaultExceptionHandler()
             .UseFastEndpoints(c =>
             {
                 c.Errors.UseProblemDetails();
                 c.Versioning.Prefix = "v";
                 c.Versioning.DefaultVersion = 1;
                 c.Versioning.PrependToRoute = true;             
             })
             .UseSwaggerGen();

        return app;

    
    }
}
