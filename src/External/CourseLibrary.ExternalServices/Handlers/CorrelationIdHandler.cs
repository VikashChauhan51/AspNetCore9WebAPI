using Microsoft.AspNetCore.Http;
using System.Diagnostics;

namespace CourseLibrary.ExternalServices.Handlers;

public sealed class CorrelationIdHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CorrelationIdHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (!request.Headers.Contains("X-Correlation-ID"))
        {
            var correlationId = Activity.Current?.TraceId.ToString()
                ?? _httpContextAccessor.HttpContext?.TraceIdentifier;

            if (!string.IsNullOrWhiteSpace(correlationId))
            {
                request.Headers.TryAddWithoutValidation("X-Correlation-ID", correlationId);
            }
        }

        return base.SendAsync(request, cancellationToken);
    }

}
