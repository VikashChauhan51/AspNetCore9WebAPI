using Microsoft.Extensions.Compliance.Classification;

namespace CourseLibrary.Infrastructure.Observability.Logging.Redaction;

public static class DataClassifications
{
    public static readonly DataClassification Secret =
        new(
            "Security",
            "Secret");


    public static readonly DataClassification Email =
        new(
            "Personal",
            "Email");


    public static readonly DataClassification Phone =
        new(
            "Personal",
            "Phone");


    public static readonly DataClassification CreditCard =
        new(
            "Financial",
            "CreditCard");


    public static readonly DataClassification PersonalData =
        new(
            "Personal",
            "General");


    public static readonly DataClassification SensitiveData =
      new(
          "Security",
          "SensitiveData");

}