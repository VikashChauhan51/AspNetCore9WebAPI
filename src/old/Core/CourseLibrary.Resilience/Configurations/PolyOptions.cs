namespace CourseLibrary.Resilience.Configurations;

public sealed class PolyOptions
{
    // Retry Policy
    public int RetryCount { get; init; } = 3;
    public int DelayInMilliseconds { get; init; } = 200;

    // Circuit Breaker Policy
    public int CircuitBreakerFailureThreshold { get; init; } = 5;
    public int CircuitBreakerDurationInSeconds { get; init; } = 30;
}

