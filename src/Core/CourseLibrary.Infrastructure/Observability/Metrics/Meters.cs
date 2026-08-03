using System.Diagnostics.Metrics;

namespace CourseLibrary.Infrastructure.Observability.Metrics;

public static class Meters
{
    public const string InfrastructureMeterName =
        "CourseLibrary.Infrastructure";

    public static readonly Meter Infrastructure =
        new(InfrastructureMeterName);
}
