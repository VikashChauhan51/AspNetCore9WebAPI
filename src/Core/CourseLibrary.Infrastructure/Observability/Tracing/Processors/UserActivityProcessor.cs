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
        RequestContextActivityTags.Apply(activity, accessor.HttpContext);
    }
}