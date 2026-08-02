using OpenTelemetry.Exporter;

namespace CourseLibrary.Logging.Telemetry.Configurations;

public sealed class OtlpExporterOptions
{
    required public Uri Endpoint { get; init; }
    required public OtlpExportProtocol Protocol { get; init; } = OtlpExportProtocol.Grpc;
}
