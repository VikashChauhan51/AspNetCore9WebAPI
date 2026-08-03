using OpenTelemetry;
using OpenTelemetry.Logs;

namespace CourseLibrary.Api.Configuration.Logging.Processors;

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

        return $"{propertyPrefix}.{key}";
    }
}