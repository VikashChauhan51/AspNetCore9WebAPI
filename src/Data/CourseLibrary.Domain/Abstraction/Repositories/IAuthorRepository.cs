using CourseLibrary.Domain.Entities;

namespace CourseLibrary.Domain.Abstraction.Repositories;

public interface IAuthorRepository
{
    void AddAuthor(Author author);
    Task<bool> AuthorExistsAsync(Guid authorId, CancellationToken cancellationToken = default);
    void DeleteAuthor(Author author);
    Task<Author?> GetAuthorAsync(Guid authorId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Author>> GetAuthorsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Author>> GetAuthorsAsync(IEnumerable<Guid> authorIds, CancellationToken cancellationToken = default);
    void UpdateAuthor(Author author);
}