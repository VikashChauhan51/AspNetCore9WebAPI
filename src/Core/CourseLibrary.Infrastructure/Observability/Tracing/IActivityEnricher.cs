using System.Diagnostics;

namespace CourseLibrary.Infrastructure.Observability.Tracing;

public interface IActivityEnricher
{
    void Enrich(Activity activity);
}
