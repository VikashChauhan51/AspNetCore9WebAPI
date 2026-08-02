using Microsoft.AspNetCore.Http;
using OpenTelemetry;
using System.Diagnostics;

namespace CourseLibrary.Infrastructure.Observability.Tracing.Processors;


public sealed class UserActivityProcessor(
    IHttpContextAccessor accessor)
    : BaseProcessor<Activity>
{
    public override void OnStart(Activity activity)
    {
        var user = accessor.HttpContext?.User;

        var userId = user?.FindFirst("sub")?.Value;

        if (userId != null)
        {
            activity.SetTag("user.id", userId);
        }
    }
}