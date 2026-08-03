using System.Diagnostics.Metrics;

namespace CourseLibrary.Api.Configuration.Telemetry.Metrics;

public static class Meters
{
    public const string DefaultMeterName =
        "CourseLibrary";

    public const string ApiMeterName =
        "CourseLibrary.Api";

    public static readonly Meter Default =
        new(DefaultMeterName);

    public static readonly Meter Api =
        new(ApiMeterName);
}