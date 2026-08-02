namespace CourseLibrary.ExternalServices.Handlers;

using Microsoft.AspNetCore.Http;

public sealed class AuthorizationHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthorizationHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // Check if the request opts out of adding the Authorization header
        request.Options.TryGetValue(new HttpRequestOptionsKey<bool>("__SkipAuthorizationHeader"), out bool skip);
        if (!skip)
        {
            var authHeader = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(authHeader) && authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                request.Headers.TryAddWithoutValidation("Authorization", authHeader);
            }
        }

        return base.SendAsync(request, cancellationToken);
    }

}
