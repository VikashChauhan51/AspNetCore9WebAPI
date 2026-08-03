using OpenTelemetry;
using System.Diagnostics;

namespace CourseLibrary.Api.Configuration.Telemetry.Tracing.Processors;

public sealed class ApplicationActivityProcessor(
    IHostEnvironment hostEnvironment)
    : BaseProcessor<Activity>
{
    public override void OnStart(Activity activity)
    {
        activity.SetTag("application.name", hostEnvironment.ApplicationName);
        activity.SetTag("application.environment", hostEnvironment.EnvironmentName);
    }
}