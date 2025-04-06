using Microsoft.AspNetCore.Http;

namespace CourseLibrary.Resilience.Handlers;

public sealed class CulturePropagationHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _accessor;

    public CulturePropagationHandler(IHttpContextAccessor accessor) => _accessor = accessor;

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var culture = _accessor.HttpContext?.Request.Headers["Accept-Language"].FirstOrDefault();
        if (!string.IsNullOrWhiteSpace(culture))
        {
            request.Headers.TryAddWithoutValidation("Accept-Language", culture);
        }

        return base.SendAsync(request, cancellationToken);
    }
}

