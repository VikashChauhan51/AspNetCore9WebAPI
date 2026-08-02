using System.Diagnostics;

namespace CourseLibrary.Infrastructure.Observability.Enrichers;

public interface IActivityEnricher
{
    void Enrich(Activity activity);
}
