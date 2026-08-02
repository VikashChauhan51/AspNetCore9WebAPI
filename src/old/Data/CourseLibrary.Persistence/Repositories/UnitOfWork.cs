using CourseLibrary.Domain.Abstraction.Repositories;
using Microsoft.EntityFrameworkCore.Storage;
using CourseLibrary.Persistence.DbContexts;

namespace CourseLibrary.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly CourseLibraryContext _context;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(CourseLibraryContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
        {
            throw new InvalidOperationException("Transaction already started.");
        }
        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken: cancellationToken);
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction == null)
        {
            throw new InvalidOperationException("No active transaction.");
        }
        await _context.SaveChangesAsync(cancellationToken: cancellationToken);
        await _transaction.CommitAsync(cancellationToken: cancellationToken);
        await _transaction.DisposeAsync();
        _transaction = null;
    }

    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction == null)
        {
            throw new InvalidOperationException("No active transaction.");
        }
        await _transaction.RollbackAsync(cancellationToken: cancellationToken);
        await _transaction.DisposeAsync();
        _transaction = null;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken: cancellationToken);
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}