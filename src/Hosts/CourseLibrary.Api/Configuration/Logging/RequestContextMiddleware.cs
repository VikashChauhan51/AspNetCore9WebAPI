using System.Diagnostics;
using CourseLibrary.Api.Configuration.Telemetry.Tracing;
using Microsoft.Extensions.Primitives;

namespace CourseLibrary.Api.Configuration.Logging;

internal sealed class RequestContextMiddleware(
    RequestDelegate next,
    ILogger<RequestContextMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var correlationId = ResolveCorrelationId(context);
        var requestId = context.TraceIdentifier;
        var safePath = GetSafeRequestPath(context.Request);
        var route = safePath;

        var activity = Activity.Current ??
            ActivitySources.Api.StartActivity("request.context");

        var scopeValues = new Dictionary<string, object?>
        {
            ["request.id"] = requestId,
            ["request.correlation_id"] = correlationId,
            ["request.method"] = context.Request.Method,
            ["request.path"] = safePath,
            ["request.host"] = context.Request.Host.Value,
            ["request.scheme"] = context.Request.Scheme,
            ["request.route"] = route,
            ["request.user_agent"] = context.Request.Headers["User-Agent"].ToString(),
            ["request.remote_ip"] = context.Connection.RemoteIpAddress?.ToString(),
            ["request.content_type"] = context.Request.ContentType,
            ["trace.id"] = activity?.TraceId.ToString()
        };

        using var scope = logger.BeginScope(scopeValues);
        RequestContextActivityTags.Apply(activity, context, correlationId, requestId, route);

        if (!context.Response.Headers.ContainsKey("X-Correlation-ID"))
        {
            context.Response.Headers["X-Correlation-ID"] = correlationId;
        }

        try
        {
            await next(context);
        }
        finally
        {
            RequestContextActivityTags.ApplyResponse(activity, context);
        }
    }

    private static string ResolveCorrelationId(HttpContext context)
    {
        if (context.Request.Headers.TryGetValue("X-Correlation-ID", out var correlationId)
            && !StringValues.IsNullOrEmpty(correlationId))
        {
            return correlationId.ToString();
        }

        if (context.Request.Headers.TryGetValue("traceparent", out var traceParent)
            && !StringValues.IsNullOrEmpty(traceParent))
        {
            return traceParent.ToString();
        }

        return context.TraceIdentifier;
    }

    private static string GetSafeRequestPath(HttpRequest request)
    {
        var path = request.Path.Value;

        return string.IsNullOrWhiteSpace(path)
            ? "/"
            : path;
    }
}
