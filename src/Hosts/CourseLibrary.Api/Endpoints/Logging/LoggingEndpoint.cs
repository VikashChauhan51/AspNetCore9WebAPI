using Carter;

namespace CourseLibrary.Api.Endpoints.Logging;

public sealed class LoggingModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/test-log", (ILogger<LoggingModule> logger) =>
        {
            var email = "vikash.chauhan@gmail.com";

            logger.UserLoggedIn(email);
            logger.LogInformation(
                "This is testing message with user {UserId}",
                100);

            return Results.Ok("Log written.");
        })
        .WithName("TestLog")
        .WithTags("Logging");
    }
}