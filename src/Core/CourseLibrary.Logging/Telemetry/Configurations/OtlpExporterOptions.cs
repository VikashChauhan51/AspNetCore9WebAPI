using OpenTelemetry.Exporter;

namespace CourseLibrary.Logging.Telemetry.Configurations;

public sealed class OtlpExporterOptions
{
    required public Uri Endpoint { get; set; }
    required public OtlpExportProtocol Protocol { get; set; } = OtlpExportProtocol.Grpc;
}
