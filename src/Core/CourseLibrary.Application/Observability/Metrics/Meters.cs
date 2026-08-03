using System.Diagnostics.Metrics;

namespace CourseLibrary.Application.Observability.Metrics;

public static class Meters
{
    public const string ApplicationMeterName =
        "CourseLibrary.Application";

    public static readonly Meter Application =
        new(ApplicationMeterName);
}
