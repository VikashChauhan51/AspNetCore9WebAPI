using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using OpenTelemetry;
using System.Diagnostics;

namespace CourseLibrary.Logging.Telemetry.Enrichments;

public class HttpContextEnrichmentProcessor : BaseProcessor<Activity>
{
    private readonly IWebHostEnvironment hostEnvironment;
    private readonly IHttpContextAccessor httpContextAccessor;

    public HttpContextEnrichmentProcessor(IWebHostEnvironment hostEnvironment, IHttpContextAccessor httpContextAccessor)
    {
        this.hostEnvironment = hostEnvironment;
        this.httpContextAccessor = httpContextAccessor;
    }

    public override void OnStart(Activity activity)
    {
        if (activity == null)
            return;

        string environment = this.hostEnvironment.EnvironmentName;
        string application = this.hostEnvironment.ApplicationName;
        string threadId = Thread.CurrentThread.ManagedThreadId.ToString();
        string? correlationId = this.httpContextAccessor?.HttpContext?.TraceIdentifier;
        string? clientIp = this.httpContextAccessor?.HttpContext?.Connection.RemoteIpAddress?.ToString();
        string? requestPath = this.httpContextAccessor?.HttpContext?.Request.Path;
        string? requestHost = this.httpContextAccessor?.HttpContext?.Request.Host.Value;
        string? userAgent = this.httpContextAccessor?.HttpContext?.Request.Headers["User-Agent"].ToString();
        string? contentType = this.httpContextAccessor?.HttpContext?.Request.ContentType;
        string? httpMethod = this.httpContextAccessor?.HttpContext?.Request.Method;
        string? scheme = this.httpContextAccessor?.HttpContext?.Request.Scheme;
        string? traceId = activity.TraceId.ToString();
        string? spanId = activity.SpanId.ToString();

        var process = Process.GetCurrentProcess();

        // System info
        var osDescription = System.Runtime.InteropServices.RuntimeInformation.OSDescription;
        var osArchitecture = System.Runtime.InteropServices.RuntimeInformation.OSArchitecture.ToString();
        var runtimeVersion = Environment.Version.ToString();
        var osPlatform = Environment.OSVersion.Platform.ToString();

        // OpenTelemetry standard semantic conventions (https://opentelemetry.io/docs/specs/semconv/)
        activity.SetTag("app.name", application);
        activity.SetTag("env.name", environment);
        activity.SetTag("thread.id", threadId);
        activity.SetTag("correlation.id", correlationId);
        activity.SetTag("net.peer.ip", clientIp);
        activity.SetTag("http.method", httpMethod);
        activity.SetTag("http.scheme", scheme);
        activity.SetTag("http.target", requestPath);
        activity.SetTag("http.host", requestHost);
        activity.SetTag("http.user_agent", userAgent);
        activity.SetTag("http.request_content_type", contentType);
        activity.SetTag("otel.trace_id", traceId);
        activity.SetTag("otel.span_id", spanId);

        // Process/system-specific tags
        activity.SetTag("process.id", process.Id);
        activity.SetTag("process.name", process.ProcessName);
        activity.SetTag("process.thread.count", process.Threads.Count);
        activity.SetTag("process.memory.working_set", process.WorkingSet64);
        activity.SetTag("os.description", osDescription);
        activity.SetTag("os.architecture", osArchitecture);
        activity.SetTag("os.platform", osPlatform);
        activity.SetTag("runtime.version", runtimeVersion);
    }
}
