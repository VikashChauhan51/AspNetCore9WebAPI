using System.Diagnostics;
using OpenTelemetry;

namespace CourseLibrary.Api.Configuration.Telemetry.Tracing.Processors;

public sealed class ExceptionActivityProcessor
    : BaseProcessor<Activity>
{
    public override void OnEnd(Activity activity)
    {
        if (activity.Status == ActivityStatusCode.Error)
        {
            activity.SetTag("activity.failed", true);
        }
    }
}
