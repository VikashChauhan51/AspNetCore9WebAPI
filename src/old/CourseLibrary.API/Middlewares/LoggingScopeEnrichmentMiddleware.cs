namespace CourseLibrary.API.Middlewares;

using System;
using System.Diagnostics;

public sealed class LoggingScopeEnrichmentMiddleware(
    RequestDelegate next,
    ILogger<LoggingScopeEnrichmentMiddleware> logger,
    IWebHostEnvironment env)
{
    public async Task InvokeAsync(HttpContext context)
    {
        string environment = env.EnvironmentName;
        string application = env.ApplicationName;
        string threadId = Thread.CurrentThread.ManagedThreadId.ToString();
        string? correlationId = context.TraceIdentifier;
        string? clientIp = context.Connection.RemoteIpAddress?.ToString();
        string? requestPath = context.Request.Path;
        string? requestHost = context.Request.Host.Value;
        string? userAgent = context.Request.Headers["User-Agent"].ToString();
        string? contentType = context.Request.ContentType;
        string? httpMethod = context.Request.Method;
        string? scheme = context.Request.Scheme;
        string? traceId = Activity.Current?.TraceId.ToString();
        string? spanId = Activity.Current?.SpanId.ToString();

        var process = Process.GetCurrentProcess();

        // System info
        var osDescription = System.Runtime.InteropServices.RuntimeInformation.OSDescription;
        var osArchitecture = System.Runtime.InteropServices.RuntimeInformation.OSArchitecture.ToString();
        var runtimeVersion = Environment.Version.ToString();
        var osPlatform = Environment.OSVersion.Platform.ToString();


        // OpenTelemetry standard semantic conventions (https://opentelemetry.io/docs/specs/semconv/)
        Activity.Current?.SetTag("app.name", application);
        Activity.Current?.SetTag("env.name", environment);
        Activity.Current?.SetTag("thread.id", threadId);
        Activity.Current?.SetTag("correlation.id", correlationId);
        Activity.Current?.SetTag("net.peer.ip", clientIp);
        Activity.Current?.SetTag("http.method", httpMethod);
        Activity.Current?.SetTag("http.scheme", scheme);
        Activity.Current?.SetTag("http.target", requestPath);
        Activity.Current?.SetTag("http.host", requestHost);
        Activity.Current?.SetTag("http.user_agent", userAgent);
        Activity.Current?.SetTag("http.request_content_type", contentType);
        Activity.Current?.SetTag("otel.trace_id", traceId);
        Activity.Current?.SetTag("otel.span_id", spanId);

        // Process/system-specific tags
        Activity.Current?.SetTag("process.id", process.Id);
        Activity.Current?.SetTag("process.name", process.ProcessName);
        Activity.Current?.SetTag("process.thread.count", process.Threads.Count);
        Activity.Current?.SetTag("process.memory.working_set", process.WorkingSet64);
        Activity.Current?.SetTag("os.description", osDescription);
        Activity.Current?.SetTag("os.architecture", osArchitecture);
        Activity.Current?.SetTag("os.platform", osPlatform);
        Activity.Current?.SetTag("runtime.version", runtimeVersion);

        var baseScope = new Dictionary<string, object?>
        {
            ["Application"] = application,
            ["Environment"] = environment,
            ["ThreadId"] = threadId,
            ["CorrelationId"] = correlationId,
            ["TraceId"] = traceId,
            ["SpanId"] = spanId,
            ["RequestPath"] = context.Request.Path,
            ["RequestMethod"] = context.Request.Method,
            ["RequestScheme"] = context.Request.Scheme,
            ["RequestHost"] = context.Request.Host.Value,
            ["UserAgent"] = context.Request.Headers["User-Agent"].ToString(),
            ["ClientIP"] = clientIp,
            ["ContentType"] = contentType,
            ["ProcessId"] = process.Id,
            ["ProcessName"] = process.ProcessName,
            ["ProcessThreadCount"] = process.Threads.Count,
            ["ProcessWorkingSet"] = process.WorkingSet64,
            ["OSDescription"] = osDescription,
            ["OSArchitecture"] = osArchitecture,
            ["OSPlatform"] = osPlatform,
            ["RuntimeVersion"] = runtimeVersion
        };

        using (logger.BeginScope(baseScope))
        {
            await next(context);
        }
    }
}


