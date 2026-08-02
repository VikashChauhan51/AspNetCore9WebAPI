using OpenTelemetry.Logs;
using System;
using System.Collections.Generic;
using System.Text;

namespace CourseLibrary.Infrastructure.Observability.Logging;

public interface ILogEnricher
{
    void Enrich(LogRecord record);
}