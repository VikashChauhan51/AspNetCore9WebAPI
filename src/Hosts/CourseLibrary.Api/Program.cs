using Carter;
using CourseLibrary.Api.Configuration.Logging;
using CourseLibrary.Api.Configuration.Telemetry;
using CourseLibrary.Api.Endpoints;
using CourseLibrary.Infrastructure.Observability.Logging.Redaction;

var builder = WebApplication.CreateBuilder(args);

builder.AddLoggingObservability();
builder.AddObservability();
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();
builder.Services.AddCarter();

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
app.MapCarter();

app.Run();

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

