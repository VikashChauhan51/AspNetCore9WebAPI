using System.Diagnostics.Metrics;

namespace CourseLibrary.Infrastructure.Observability.Metrics;

public static class ApplicationMetrics
{
    public static readonly Counter<long> CoursesCreated =
    Meters.Application.CreateCounter<long>(
        MetricNames.CoursesCreated);
}
