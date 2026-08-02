using OpenTelemetry;
using OpenTelemetry.Logs;

namespace CourseLibrary.Infrastructure.Observability.Logging.Processors;

public sealed class CourseLibraryLogProcessor(
    string propertyPrefix = "courselibrary")
    : BaseProcessor<LogRecord>
{
    public override void OnEnd(LogRecord logRecord)
    {
        if (logRecord.Attributes is null ||
            logRecord.Attributes.Count == 0)
        {
            return;
        }

        var attributes = logRecord.Attributes;
        var updated = new List<KeyValuePair<string, object?>>(attributes.Count);

        foreach (var attribute in attributes)
        {
            updated.Add(
                new KeyValuePair<string, object?>(
                    NormalizeKey(attribute.Key),
                    attribute.Value));
        }

        logRecord.Attributes = updated;
    }

    private string NormalizeKey(string key)
    {
        // Preserve OpenTelemetry semantic attributes.
        if (key.Contains('.'))
        {
            return key;
        }

        if (key == "{OriginalFormat}")
        {
            return key;
        }

        return $"{propertyPrefix}.{ToDotCase(key)}";
    }

    private static string ToDotCase(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return value;
        }

        var builder = new System.Text.StringBuilder(value.Length + 8);

        for (var i = 0; i < value.Length; i++)
        {
            var c = value[i];

            if (char.IsUpper(c))
            {
                if (i > 0)
                {
                    builder.Append('.');
                }

                builder.Append(char.ToLowerInvariant(c));
            }
            else
            {
                builder.Append(c);
            }
        }

        return builder.ToString();
    }
}