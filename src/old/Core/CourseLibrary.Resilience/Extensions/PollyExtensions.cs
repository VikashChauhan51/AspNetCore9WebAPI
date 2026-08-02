namespace CourseLibrary.Resilience.Extensions;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using CourseLibrary.Resilience.Configurations;
using CourseLibrary.Resilience.Policies;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Refit;
using Microsoft.Extensions.Http;
using Polly;

public static class PollyExtensions
{
    public static IServiceCollection ConfigureResilience(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<PolyOptions>(configuration.GetSection("Polly"));
        return services;
    }

    public static IServiceCollection AddResilienceHttpClient<TClient, TImplementation>(
        this IServiceCollection services,
        string baseAddress)
        where TClient : class
        where TImplementation : class, TClient
    {
        ArgumentNullException.ThrowIfNull(baseAddress, nameof(baseAddress));

        services.AddHttpClient<TClient, TImplementation>(client =>
        {
            client.BaseAddress = new Uri(baseAddress);
        })
        .AddPolicyHandler((sp, _) =>
        {
            var logger = sp.GetRequiredService<ILogger<TImplementation>>();
            var options = sp.GetRequiredService<IOptions<PolyOptions>>().Value;
            return PolicyFactory.CreateRetryPolicy(options, logger);
        })
        .AddPolicyHandler((sp, _) =>
        {
            var logger = sp.GetRequiredService<ILogger<TImplementation>>();
            var options = sp.GetRequiredService<IOptions<PolyOptions>>().Value;
            return PolicyFactory.CreateCircuitBreakerPolicy(options, logger);
        });

        return services;
    }

    public static IServiceCollection AddResilienceNamedHttpClient(
        this IServiceCollection services,
        string clientName,
        string baseAddress)
    {
        ArgumentNullException.ThrowIfNull(clientName, nameof(clientName));
        ArgumentNullException.ThrowIfNull(baseAddress, nameof(baseAddress));

        services.AddHttpClient(clientName, client =>
        {
            client.BaseAddress = new Uri(baseAddress);
        })
        .AddPolicyHandler((sp, _) =>
        {
            var logger = sp.GetRequiredService<ILoggerFactory>().CreateLogger(clientName);
            var options = sp.GetRequiredService<IOptions<PolyOptions>>().Value;
            return PolicyFactory.CreateRetryPolicy(options, logger);
        })
        .AddPolicyHandler((sp, _) =>
        {
            var logger = sp.GetRequiredService<ILoggerFactory>().CreateLogger(clientName);
            var options = sp.GetRequiredService<IOptions<PolyOptions>>().Value;
            return PolicyFactory.CreateCircuitBreakerPolicy(options, logger);
        });

        return services;
    }

    public static IServiceCollection AddResilienceRefitClient<TClient>(
        this IServiceCollection services,
        string baseAddress)
        where TClient : class
    {
        ArgumentNullException.ThrowIfNull(baseAddress, nameof(baseAddress));

        services.AddRefitClient<TClient>()
            .ConfigureHttpClient(client =>
            {
                client.BaseAddress = new Uri(baseAddress);
            })
            .AddPolicyHandler((sp, _) =>
            {
                var logger = sp.GetRequiredService<ILogger<TClient>>();
                var options = sp.GetRequiredService<IOptions<PolyOptions>>().Value;
                return PolicyFactory.CreateRetryPolicy(options, logger);
            })
            .AddPolicyHandler((sp, _) =>
            {
                var logger = sp.GetRequiredService<ILogger<TClient>>();
                var options = sp.GetRequiredService<IOptions<PolyOptions>>().Value;
                return PolicyFactory.CreateCircuitBreakerPolicy(options, logger);
            });

        return services;
    }

    public static HttpClient CreateResilientClient(
        this IHttpClientFactory httpClientFactory,
        IServiceProvider serviceProvider,
        string clientName,
        ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(httpClientFactory);
        ArgumentNullException.ThrowIfNull(serviceProvider);
        ArgumentNullException.ThrowIfNull(logger);

        var options = serviceProvider.GetRequiredService<IOptions<PolyOptions>>().Value;

        // Create the raw client
        var client = httpClientFactory.CreateClient();

        // Create policies (retry + circuit breaker)
        var retryPolicy = PolicyFactory.CreateRetryPolicy(options, logger);
        var circuitBreakerPolicy = PolicyFactory.CreateCircuitBreakerPolicy(options, logger);

        // Wrap them into a policy pipeline
        var policyWrap = Policy.WrapAsync(retryPolicy, circuitBreakerPolicy);

        // Create a delegating handler using Polly
        var handler = new PolicyHttpMessageHandler(policyWrap)
        {
            InnerHandler = new HttpClientHandler()
        };

        // Return a client with policy pipeline
        return new HttpClient(handler)
        {
            BaseAddress = client.BaseAddress,
            Timeout = client.Timeout
        };
    }

    public static HttpClient CreateResilientClient(
       this IHttpClientFactory httpClientFactory,
       PolyOptions options,
       string clientName,
       ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(httpClientFactory);
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(logger);
        // Create the raw client
        var client = httpClientFactory.CreateClient();

        // Create policies (retry + circuit breaker)
        var retryPolicy = PolicyFactory.CreateRetryPolicy(options, logger);
        var circuitBreakerPolicy = PolicyFactory.CreateCircuitBreakerPolicy(options, logger);

        // Wrap them into a policy pipeline
        var policyWrap = Policy.WrapAsync(retryPolicy, circuitBreakerPolicy);

        // Create a delegating handler using Polly
        var handler = new PolicyHttpMessageHandler(policyWrap)
        {
            InnerHandler = new HttpClientHandler()
        };

        // Return a client with policy pipeline
        return new HttpClient(handler)
        {
            BaseAddress = client.BaseAddress,
            Timeout = client.Timeout
        };
    }

    public static IHttpClientBuilder ConfigureHttpClientRetryPolicy(this IHttpClientBuilder builder)
    {
        return builder
             .AddPolicyHandler((sp, _) =>
             {
                 var logger = sp.GetRequiredService<ILogger>();
                 var options = sp.GetRequiredService<IOptions<PolyOptions>>().Value;
                 return PolicyFactory.CreateRetryPolicy(options, logger);
             })
            .AddPolicyHandler((sp, _) =>
            {
                var logger = sp.GetRequiredService<ILogger>();
                var options = sp.GetRequiredService<IOptions<PolyOptions>>().Value;
                return PolicyFactory.CreateCircuitBreakerPolicy(options, logger);
            });
    }

}


