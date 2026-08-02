using System.Diagnostics.Metrics;

namespace CourseLibrary.Infrastructure.Observability.Metrics;

public static class HttpMetrics
{
    public static readonly Counter<long> RateLimitExceeded =
        Meters.Default.CreateCounter<long>("http.rate_limit.exceeded");
}
