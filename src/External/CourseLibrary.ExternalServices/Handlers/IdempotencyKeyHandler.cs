using Microsoft.AspNetCore.Http;

namespace CourseLibrary.ExternalServices.Handlers;

public sealed class IdempotencyKeyHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public IdempotencyKeyHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // Only apply to HTTP methods where idempotency is meaningful
        if (request.Method == HttpMethod.Post ||
            request.Method == HttpMethod.Put ||
            request.Method == HttpMethod.Patch)
        {
            const string headerName = "X-Idempotency-Key";

            // Check if already set manually (e.g., in calling code)
            if (!request.Headers.Contains(headerName))
            {
                // Option 1: Use header from incoming request
                var key = _httpContextAccessor.HttpContext?.Request.Headers[headerName].FirstOrDefault();

                // Option 2: Generate one if not present
                if (string.IsNullOrWhiteSpace(key))
                    key = Guid.NewGuid().ToString("N");

                request.Headers.TryAddWithoutValidation(headerName, key);
            }
        }

        return base.SendAsync(request, cancellationToken);
    }
}

