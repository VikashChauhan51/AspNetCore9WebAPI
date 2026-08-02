using Microsoft.Extensions.Compliance.Classification;
using System;
using System.Collections.Generic;
using System.Text;

namespace CourseLibrary.Infrastructure.Observability.Logging.Redaction;

[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property)]
public sealed class EmailAttribute : DataClassificationAttribute
{
    public EmailAttribute()
        : base(DataClassifications.Email)
    {
    }
}