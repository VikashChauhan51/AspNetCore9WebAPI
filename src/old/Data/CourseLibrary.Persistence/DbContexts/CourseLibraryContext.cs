using CourseLibrary.Domain.Entities;
using CourseLibrary.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace CourseLibrary.Persistence.DbContexts;

public class CourseLibraryContext : DbContext
{
    public CourseLibraryContext(DbContextOptions<CourseLibraryContext> options)
       : base(options)
    {
        base.ChangeTracker.LazyLoadingEnabled = false;
        base.ChangeTracker.AutoDetectChangesEnabled = false;
    }

    public DbSet<Author> Authors { get; set; } = null!;
    public DbSet<Course> Courses { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AuthorConfiguration).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
