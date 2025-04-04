namespace CourseLibrary.Logging.Telemetry.Exporters;

using OpenTelemetry;
using System.Diagnostics;
using System.Globalization;
using System.Text;

public class CsvActivityExporter : BaseExporter<Activity>
{
    private readonly string _filePath;
    private readonly object _lock = new();

    public CsvActivityExporter(string filePath)
    {
        _filePath = filePath;

        // Initialize file with headers
        if (!File.Exists(_filePath))
        {
            File.WriteAllText(_filePath, GetCsvHeader() + Environment.NewLine);
        }
    }

    public override ExportResult Export(in Batch<Activity> batch)
    {
        var sb = new StringBuilder();

        foreach (var activity in batch)
        {
            sb.AppendLine(FormatActivityAsCsv(activity));
        }

        lock (_lock)
        {
            File.AppendAllText(_filePath, sb.ToString());
        }

        return ExportResult.Success;
    }

    private static string GetCsvHeader()
    {
        return "TraceId,SpanId,ParentSpanId,StartTime,DurationMs,Name,StatusCode,Tags";
    }

    private static string FormatActivityAsCsv(Activity activity)
    {
        var tags = string.Join(";", activity.Tags.Select(tag => $"{tag.Key}={tag.Value}"));
        return string.Join(",", new[]
        {
            activity.TraceId.ToString(),
            activity.SpanId.ToString(),
            activity.ParentSpanId.ToString(),
            activity.StartTimeUtc.ToString("o", CultureInfo.InvariantCulture),
            activity.Duration.TotalMilliseconds.ToString(CultureInfo.InvariantCulture),
            EscapeCsv(activity.DisplayName),
            activity.Status.ToString(),
            EscapeCsv(tags)
        });
    }

    private static string EscapeCsv(string value)
    {
        if (value.Contains(',') || value.Contains('"'))
        {
            return $"\"{value.Replace("\"", "\"\"")}\"";
        }

        return value;
    }
}
