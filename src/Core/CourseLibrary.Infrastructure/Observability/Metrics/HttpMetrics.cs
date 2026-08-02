using System.Diagnostics.Metrics;

namespace CourseLibrary.Infrastructure.Observability.Metrics;

public static class HttpMetrics
{
    public static readonly Counter<long> ApiRateLimitExceeded =
        Meters.Api.CreateCounter<long>("http.rate_limit.exceeded");
}