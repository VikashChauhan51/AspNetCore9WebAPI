using CourseLibrary.Authentication;
using CourseLibrary.Authentication.Configurations;

namespace CourseLibrary.API.Extensions;

public static class AuthenticationExtensions
{
    public static void ConfigureAuthentication(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<AuthenticationConfiguration>(
        builder.Configuration.GetSection("Authentication"));
        builder.Services.AddAuthorization();
        builder.Services.AddAuthentication();
        builder.Services.AddCors();
        builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
    }
}
