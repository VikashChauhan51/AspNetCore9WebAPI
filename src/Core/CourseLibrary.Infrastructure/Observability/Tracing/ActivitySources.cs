using System.Diagnostics;

namespace CourseLibrary.Infrastructure.Observability.Tracing;

public static class ActivitySources
{
    public const string InfrastructureSourceName =
        "CourseLibrary.Infrastructure";

    public static readonly ActivitySource Infrastructure =
        new(InfrastructureSourceName);
}
