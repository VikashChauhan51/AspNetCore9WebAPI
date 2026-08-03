using System.Diagnostics;
using OpenTelemetry;

namespace CourseLibrary.Api.Configuration.Telemetry.Tracing.Processors;

public sealed class CorrelationActivityProcessor
    : BaseProcessor<Activity>
{
    public override void OnStart(Activity activity)
    {
        activity.SetTag("trace.id", activity.TraceId.ToString());
        activity.SetTag("span.id", activity.SpanId.ToString());

        if (activity.ParentSpanId != default)
        {
            activity.SetTag("parent.span.id", activity.ParentSpanId.ToString());
        }
    }
}
