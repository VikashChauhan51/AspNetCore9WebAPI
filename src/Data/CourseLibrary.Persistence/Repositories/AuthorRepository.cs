using CourseLibrary.Domain.Abstraction.Repositories;
using CourseLibrary.Domain.Entities;
using CourseLibrary.Persistence.DbContexts;
using Dapper;
using Microsoft.EntityFrameworkCore;

namespace CourseLibrary.Persistence.Repositories;

public class AuthorRepository : IAuthorRepository
{
    protected readonly CourseLibraryContext _context;
    public AuthorRepository(CourseLibraryContext context)  
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public void AddAuthor(Author author)
    {
        if (author == null)
        {
            throw new ArgumentNullException(nameof(author));
        }
        author.Id = Guid.NewGuid();

        foreach (var course in author.Courses)
        {
            course.Id = Guid.NewGuid();
            course.Author = author;
            course.AuthorId = author.Id;
        }

        _context.Authors.Add(author);
    }

    public async Task<bool> AuthorExistsAsync(Guid authorId, CancellationToken cancellationToken = default)
    {
        if (authorId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(authorId));
        }

        const string sql = @"
        SELECT 1 
        WHERE EXISTS (
            SELECT 1 FROM Authors WHERE Id = @authorId
        );
    ";
        int? result = await _context.Database.GetDbConnection()
            .QueryFirstOrDefaultAsync<int?>(
            new CommandDefinition(sql,
            new { authorId },
            cancellationToken: cancellationToken));

        return result.HasValue;
    }

    public void DeleteAuthor(Author author)
    {
        if (author == null)
        {
            throw new ArgumentNullException(nameof(author));
        }

        _context.Authors.Remove(author);
    }

    public async Task<Author?> GetAuthorAsync(Guid authorId, CancellationToken cancellationToken = default)
    {
        if (authorId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(authorId));
        }
        return await _context.Authors.Include(x=>x.Courses).FirstOrDefaultAsync(a => a.Id == authorId);
    }


    public async Task<IEnumerable<Author>> GetAuthorsAsync(CancellationToken cancellationToken = default)
    {
        const string sql = "Select * from Authors";
        return await _context.Database.GetDbConnection().QueryAsync<Author>(new CommandDefinition(sql, cancellationToken: cancellationToken));
    }

    public async Task<IEnumerable<Author>> GetAuthorsAsync(IEnumerable<Guid> authorIds, CancellationToken cancellationToken = default)
    {
        if (authorIds == null)
        {
            throw new ArgumentNullException(nameof(authorIds));
        }

        return await _context.Authors.Where(a => authorIds.Contains(a.Id))
            .OrderBy(a => a.FirstName)
            .OrderBy(a => a.LastName)
            .ToListAsync();
    }

    public void UpdateAuthor(Author author)
    {
        _context.Authors.Update(author);
    }


}
