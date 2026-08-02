using CourseLibrary.Infrastructure.Observability.Logging.Redaction;
using Microsoft.Extensions.Compliance.Classification;
using Microsoft.Extensions.Compliance.Redaction;
using OpenTelemetry;
using OpenTelemetry.Logs;

namespace CourseLibrary.Infrastructure.Observability.Logging.Processors;

public sealed class RedactionLogProcessor(
    IRedactorProvider redactorProvider)
    : BaseProcessor<LogRecord>
{
    public override void OnEnd(LogRecord logRecord)
    {
        if (logRecord.Attributes is null || logRecord.Attributes.Count == 0)
        {
            return;
        }

        var originalAttributes = logRecord.Attributes;
        var redactedAttributes = new List<KeyValuePair<string, object?>>(originalAttributes.Count);

        foreach (var attribute in originalAttributes)
        {
            if (attribute.Value is SensitiveValue sensitive)
            {
                var redactor = redactorProvider.GetRedactor(
                    new DataClassificationSet(sensitive.Classification));

                var redactedValue = redactor.Redact(sensitive.Value?.ToString() ?? string.Empty);
                redactedAttributes.Add(
                    new KeyValuePair<string, object?>(attribute.Key, redactedValue));
            }
            else
            {
                // Keep non‑sensitive values unchanged
                redactedAttributes.Add(attribute);
            }
        }

        // Replace the entire attribute list
        logRecord.Attributes = redactedAttributes;
    }
}