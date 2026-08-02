using CourseLibrary.Infrastructure.Observability.Logging;
using CourseLibrary.Infrastructure.Observability.Logging.Processors;
using CourseLibrary.Infrastructure.Observability.Logging.Redaction;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Compliance.Redaction;

namespace CourseLibrary.Api.Configuration.Logging;

internal static class LoggingExtensions
{
    public static WebApplicationBuilder AddLoggingObservability(
       this WebApplicationBuilder builder)
    {
        builder.Logging.EnableRedaction();
        builder.Services.AddRedaction(options =>
        {
            // Passwords, tokens, secrets
            options.SetRedactor<ErasingRedactor>(
                DataClassifications.Secret);


            // Email masking
            options.SetRedactor<EmailRedactor>(
                DataClassifications.Email);


            // General personal information
            // Example:
            // name, customer reference, identifiers
            options.SetRedactor<PartialMaskingRedactor>(
                DataClassifications.PersonalData);


            // Values where correlation is useful
            // Example:
            // tenant id, external reference id
            options.SetRedactor<HmacRedactor>(
                DataClassifications.SensitiveData);

        });

        builder.Services.AddSingleton<CourseLibraryLogProcessor>();
        builder.Services.AddHttpContextAccessor();

        return builder;
    }

    public static IApplicationBuilder UseRequestContext(this IApplicationBuilder app)
    {
        app.UseMiddleware<RequestContextMiddleware>();
        app.UseMiddleware<UserContextMiddleware>();

        return app;
    }
}