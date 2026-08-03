using System.Diagnostics.Metrics;

namespace CourseLibrary.Api.Configuration.Telemetry.Metrics;

public static class HttpMetrics
{
    public static readonly Counter<long> ApiRateLimitExceeded =
        Meters.Api.CreateCounter<long>("http.rate_limit.exceeded");
}