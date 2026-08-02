using System.Diagnostics;

namespace CourseLibrary.Infrastructure.Observability.Telemetry;

public static class ActivitySources
{
    public const string DefaultSourceName =
        "CourseLibrary";

    public const string ApiSourceName =
        "CourseLibrary.Api";

    public const string ApplicationSourceName =
        "CourseLibrary.Application";

    public const string InfrastructureSourceName =
        "CourseLibrary.Infrastructure";


    public static readonly ActivitySource Default =
        new(DefaultSourceName);

    public static readonly ActivitySource Api =
        new(ApiSourceName);

    public static readonly ActivitySource Application =
        new(ApplicationSourceName);

    public static readonly ActivitySource Infrastructure =
        new(InfrastructureSourceName);
}