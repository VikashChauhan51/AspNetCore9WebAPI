using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace CourseLibrary.Infrastructure.Observability.Tracing;

public static class RequestContextActivityTags
{
    public static void Apply(
        Activity? activity,
        HttpContext? httpContext,
        string? correlationId = null,
        string? requestId = null,
        string? route = null)
    {
        if (activity is null || httpContext is null)
        {
            return;
        }

        var resolvedCorrelationId = correlationId ?? ResolveCorrelationId(httpContext);
        var resolvedRequestId = requestId ?? httpContext.TraceIdentifier;
        var safePath = GetSafeRequestPath(httpContext.Request);
        var resolvedRoute = route
            ?? safePath
            ?? "unknown";

        var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? httpContext.User.FindFirst("sub")?.Value;

        activity.SetTag("request.id", resolvedRequestId);
        activity.SetTag("request.correlation_id", resolvedCorrelationId);
        activity.SetTag("request.method", httpContext.Request.Method);
        activity.SetTag("request.path", safePath);
        activity.SetTag("request.route", resolvedRoute);
        activity.SetTag("request.host", httpContext.Request.Host.Value ?? string.Empty);
        activity.SetTag("request.scheme", httpContext.Request.Scheme);
        activity.SetTag("request.user_agent", httpContext.Request.Headers["User-Agent"].ToString());
        activity.SetTag("request.remote_ip", httpContext.Connection.RemoteIpAddress?.ToString());
        activity.SetTag("http.request.method", httpContext.Request.Method);
        activity.SetTag("http.request.path", safePath);
        activity.SetTag("http.request.host", httpContext.Request.Host.Value ?? string.Empty);
        activity.SetTag("http.request.scheme", httpContext.Request.Scheme);
        activity.SetTag("client.address", httpContext.Connection.RemoteIpAddress?.ToString());

        if (!string.IsNullOrWhiteSpace(userId))
        {
            activity.SetTag("user.id", userId);
        }
    }

    public static void ApplyResponse(Activity? activity, HttpContext? httpContext)
    {
        if (activity is null || httpContext is null)
        {
            return;
        }

        activity.SetTag("http.response.status_code", httpContext.Response.StatusCode);
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
