using System.Diagnostics;

namespace CourseLibrary.Application.Observability.Tracing;

public static class ActivitySources
{
    public const string ApplicationSourceName =
        "CourseLibrary.Application";

    public static readonly ActivitySource Application =
        new(ApplicationSourceName);
}
