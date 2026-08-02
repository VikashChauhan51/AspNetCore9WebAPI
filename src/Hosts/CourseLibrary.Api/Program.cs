using CourseLibrary.Api.Configuration.Logging;
using CourseLibrary.Api.Configuration.OpenTelemetry;
using CourseLibrary.Infrastructure.Observability.Logging.Redaction;
using Microsoft.Extensions.Compliance.Classification;

var builder = WebApplication.CreateBuilder(args);

builder.AddLoggingObservability();
builder.AddObservability();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}

app.UseHttpsRedirection();
app.UseRequestContext();
app.UseAuthentication();
app.UseAuthorization();
app.UseUserContext();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};


app.MapGet("/test-log", (ILogger<Program> logger) =>
{
    var email = "vikash.chauhan@gmail.com";

    logger.UserLoggedIn(email);

    return Results.Ok("Log written.");
});

app.MapGet("/weatherforecast", () =>
{

    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}


public static partial class UserLogs
{
    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Information,
        Message = "User email: {Email}")]
    public static partial void UserLoggedIn(
        this ILogger logger,

        [Email]
        string email);
}

