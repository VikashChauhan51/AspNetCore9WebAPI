using CourseLibrary.Domain.Entities;

namespace CourseLibrary.Authentication;

public interface IJwtTokenService
{
    string? GetToken(User user);
}