using CourseLibrary.Domain.Abstraction.Repositories;
using CourseLibrary.Domain.Entities;
using CourseLibrary.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace CourseLibrary.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    protected readonly CourseLibraryContext _context;
    public UserRepository(CourseLibraryContext context) 
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<User?> GetUserAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(email))
        {
            throw new ArgumentNullException(nameof(email));
        }

        if (string.IsNullOrEmpty(password))
        {
            throw new ArgumentNullException(nameof(password));
        }

        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);

    }
    public async Task<User?> GetUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(userId));
        }
        return await _context.Users.FirstOrDefaultAsync(a => a.Id == userId);
    }
}
