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

    public async Task BeginTransactionAsync()
    {
        if (_transaction != null)
        {
            throw new InvalidOperationException("Transaction already started.");
        }
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitAsync()
    {
        if (_transaction == null)
        {
            throw new InvalidOperationException("No active transaction.");
        }
        await _context.SaveChangesAsync();
        await _transaction.CommitAsync();
        await _transaction.DisposeAsync();
        _transaction = null;
    }

    public async Task RollbackAsync()
    {
        if (_transaction == null)
        {
            throw new InvalidOperationException("No active transaction.");
        }
        await _transaction.RollbackAsync();
        await _transaction.DisposeAsync();
        _transaction = null;
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}