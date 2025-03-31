using FastEndpoints;

namespace CourseLibrary.API.Features.Authentications.Login;

public record LoginRequest(string Email, string Password) : ICommand<LoginResponse?>;

public record LoginResponse(string Token);