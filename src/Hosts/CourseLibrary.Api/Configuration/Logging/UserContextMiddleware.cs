using System.Diagnostics;
using CourseLibrary.Api.Configuration.Telemetry.Tracing;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace CourseLibrary.Api.Configuration.Logging;

internal sealed class UserContextMiddleware(
    RequestDelegate next,
    ILogger<UserContextMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var userId = ResolveUserId(context);

        if (!string.IsNullOrWhiteSpace(userId))
        {
            using var scope = logger.BeginScope(new Dictionary<string, object?>
            {
                ["user.id"] = userId
            });

            var activity = Activity.Current ?? ActivitySources.Api.StartActivity("user.context");
            RequestContextActivityTags.Apply(activity, context, route: context.Request.Path.Value);
            RequestContextActivityTags.ApplyUserId(activity, userId);
        }

        await next(context);
    }

    private static string? ResolveUserId(HttpContext context)
    {
        if (context.User?.Identity?.IsAuthenticated != true)
        {
            return null;
        }

        return context.User.FindFirst("sub")?.Value
            ?? context.User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
    }
}
