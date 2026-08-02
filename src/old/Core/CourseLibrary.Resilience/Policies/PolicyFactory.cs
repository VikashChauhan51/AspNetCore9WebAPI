namespace CourseLibrary.Resilience.Policies;

using Polly;
using CourseLibrary.Resilience.Configurations;
using Polly.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using Microsoft.Data.SqlClient;

public static class PolicyFactory
{
    public static IAsyncPolicy<HttpResponseMessage> CreateRetryPolicy(PolyOptions options, ILogger logger)
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(response => response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
            .WaitAndRetryAsync(
                retryCount: options.RetryCount,
                sleepDurationProvider: (retryAttempt) => TimeSpan.Zero,
                 onRetryAsync: async (outcome, timespan, retryAttempt, context) =>
                 {
                     TimeSpan delay = TimeSpan.FromMilliseconds(options.DelayInMilliseconds); // Default delay

                     if (outcome.Result != null && outcome.Result.Headers.TryGetValues("Retry-After", out var values))
                     {
                         var retryAfterValue = values.FirstOrDefault();
                         if (int.TryParse(retryAfterValue, out var seconds))
                         {
                             delay = TimeSpan.FromSeconds(seconds);
                         }
                         else if (DateTimeOffset.TryParse(retryAfterValue, out var retryAfterDate))
                         {
                             var calculatedDelay = retryAfterDate - DateTimeOffset.UtcNow;
                             if (calculatedDelay > TimeSpan.Zero)
                             {
                                 delay = calculatedDelay;
                             }
                         }
                     }

                     logger.LogWarning("Retry {RetryAttempt} after {Delay} due to {Reason}",
                         retryAttempt,
                         delay,
                         outcome.Exception?.Message ?? outcome.Result?.StatusCode.ToString());

                     await Task.Delay(delay);
                 });
    }

    public static IAsyncPolicy<HttpResponseMessage> CreateCircuitBreakerPolicy(PolyOptions options, ILogger logger)
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking: options.CircuitBreakerFailureThreshold,
                durationOfBreak: TimeSpan.FromSeconds(options.CircuitBreakerDurationInSeconds),
                onBreak: (outcome, duration) =>
                {
                    logger.LogError("Circuit broken due to {Reason}. Blocking for {Duration} seconds",
                        outcome.Exception?.Message ?? outcome.Result?.StatusCode.ToString(),
                        duration.TotalSeconds);
                },
                onReset: () => logger.LogInformation("Circuit closed. Requests flow resumed."),
                onHalfOpen: () => logger.LogInformation("Circuit in half-open. Trial request will be sent."));
    }
    public static IAsyncPolicy CreateSqlRetryPolicy(PolyOptions options, ILogger logger)
    {
        return Policy
            .Handle<SqlException>() // Transient SQL issues (e.g., deadlocks, connection drop)
            .Or<TimeoutException>() // General timeout
            .WaitAndRetryAsync(
                retryCount: options.RetryCount,
                sleepDurationProvider: attempt => TimeSpan.FromMilliseconds(options.DelayInMilliseconds),
                onRetry: (exception, timeSpan, retryAttempt, context) =>
                {
                    logger.LogWarning(exception,
                        "SQL Retry {RetryAttempt} after {Delay}ms due to {ExceptionType}",
                        retryAttempt, timeSpan.TotalMilliseconds, exception.GetType().Name);
                });
    }
}


