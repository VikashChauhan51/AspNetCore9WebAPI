using System.Diagnostics;

namespace CourseLibrary.Api.Configuration.OpenTelemetry.Tracing;

public static class ActivitySources
{
    public const string DefaultSourceName =
        "CourseLibrary";

    public const string ApiSourceName =
        "CourseLibrary.Api";

    public static readonly ActivitySource Default =
        new(DefaultSourceName);

    public static readonly ActivitySource Api =
        new(ApiSourceName);
}