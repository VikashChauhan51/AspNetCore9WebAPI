namespace CourseLibrary.Logging.Telemetry.Filters;

using OpenTelemetry.Trace;

public static class OpenTelemetryFilters
{
    /// <summary>
    /// Configures the OpenTelemetry ASP.NET Core instrumentation to ignore tracing for Swagger, health checks, and static framework endpoints.
    /// </summary>
    /// <param name="builder">The TracerProviderBuilder instance.</param>
    /// <returns>The same TracerProviderBuilder for chaining.</returns>
    public static TracerProviderBuilder AddAspNetCoreInstrumentationFilterForSwaggerAndHealth(this TracerProviderBuilder builder)
    {
        return builder.AddAspNetCoreInstrumentation(options =>
        {
            options.Filter = context =>
            {
                var path = context.Request.Path.Value?.ToLowerInvariant();

                if (string.IsNullOrWhiteSpace(path))
                    return true;

                return !(
                    path.StartsWith("/swagger") ||
                    path.StartsWith("/health") ||
                    path.StartsWith("/healthz") ||
                    path.StartsWith("/ready") ||
                    path.StartsWith("/_framework")
                );
            };
        });
    }
}


