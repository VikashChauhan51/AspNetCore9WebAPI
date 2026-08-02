using CourseLibrary.Domain.Entities;

namespace CourseLibrary.Domain.Abstraction.Repositories;

public interface IUserRepository
{
    Task<User?> GetUserAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<User?> GetUserAsync(string email, string password, CancellationToken cancellationToken = default);
}