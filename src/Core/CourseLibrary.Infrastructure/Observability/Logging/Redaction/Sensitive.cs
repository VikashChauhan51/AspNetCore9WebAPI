using Microsoft.Extensions.Compliance.Classification;

namespace CourseLibrary.Infrastructure.Observability.Logging.Redaction;

public static class Sensitive
{
    public static SensitiveValue Secret(string? value) =>
        new(value, DataClassifications.Secret);

    public static SensitiveValue Email(string? value) =>
        new(value, DataClassifications.Email);

    public static SensitiveValue Phone(string? value) =>
        new(value, DataClassifications.Phone);

    public static SensitiveValue CreditCard(string? value) =>
        new(value, DataClassifications.CreditCard);

    public static SensitiveValue Personal(string? value) =>
        new(value, DataClassifications.PersonalData);

    public static SensitiveValue SensitiveData(string? value) =>
        new(value, DataClassifications.SensitiveData);
}