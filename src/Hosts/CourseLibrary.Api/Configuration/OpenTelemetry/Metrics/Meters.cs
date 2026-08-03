using System.Diagnostics.Metrics;

namespace CourseLibrary.Api.Configuration.OpenTelemetry.Metrics;

public static class Meters
{
    public const string DefaultMeterName =
        "CourseLibrary";

    public const string ApiMeterName =
        "CourseLibrary.Api";

    public const string ApplicationMeterName =
        "CourseLibrary.Application";

    public const string InfrastructureMeterName =
        "CourseLibrary.Infrastructure";


    public static readonly Meter Default =
        new(DefaultMeterName);

    public static readonly Meter Api =
        new(ApiMeterName);

    public static readonly Meter Application =
        new(ApplicationMeterName);

    public static readonly Meter Infrastructure =
        new(InfrastructureMeterName);
}