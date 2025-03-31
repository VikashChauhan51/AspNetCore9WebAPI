﻿using FastEndpoints;
using FastEndpoints.Swagger;
using Serilog;

namespace CourseLibrary.API.Extensions;

public static class HostBuildingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        Log.Logger = new LoggerConfiguration()
      .WriteTo.Console()
      .Enrich.FromLogContext()
      .CreateLogger();

        builder.Host.UseSerilog(Log.Logger);

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
