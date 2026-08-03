using System.Diagnostics;
using System.Runtime.InteropServices;
using OpenTelemetry;

namespace CourseLibrary.Api.Configuration.Telemetry.Tracing.Processors;

public sealed class EnvironmentActivityProcessor
    : BaseProcessor<Activity>
{
    public override void OnStart(Activity activity)
    {
        activity.SetTag("host.name", Environment.MachineName);
        activity.SetTag("process.id", Environment.ProcessId);
        activity.SetTag("process.runtime", RuntimeInformation.FrameworkDescription);
        activity.SetTag("os.description", RuntimeInformation.OSDescription);
        activity.SetTag("os.architecture", RuntimeInformation.OSArchitecture.ToString());
        activity.SetTag("process.architecture", RuntimeInformation.ProcessArchitecture.ToString());
    }
}
