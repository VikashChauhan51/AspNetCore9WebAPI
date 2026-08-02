namespace CourseLibrary.Logging.Loggers.Enrichments;

using Microsoft.AspNetCore.Http;
using Serilog.Core;
using Serilog.Events;
using System;

public class HttpContextEnricher : ILogEventEnricher
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpContextEnricher(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }

    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var context = _httpContextAccessor.HttpContext;

        if (context == null)
            return;

        var request = context.Request;

        logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("RequestPath", request.Path));
        logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("RequestMethod", request.Method));
        logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("RequestScheme", request.Scheme));
        logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("RequestHost", request.Host.Value));
        logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("UserAgent", request.Headers["User-Agent"].ToString()));

        if (context.User?.Identity?.IsAuthenticated == true)
        {
            logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("User", context.User.Identity.Name));
        }
    }
}

