namespace CourseLibrary.Resilience.Configurations;

public sealed class PolyOptions
{
    // Retry Policy
    public int RetryCount { get; set; } = 3;
    public int DelayInMilliseconds { get; set; } = 200;

    // Circuit Breaker Policy
    public int CircuitBreakerFailureThreshold { get; set; } = 5;
    public int CircuitBreakerDurationInSeconds { get; set; } = 30;
}

