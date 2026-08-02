using Microsoft.Extensions.Compliance.Classification;

namespace CourseLibrary.Infrastructure.Observability.Logging.Redaction;

public sealed record SensitiveValue(
    object? Value,
    DataClassification Classification)
{
    public override string? ToString() => Value?.ToString();
}