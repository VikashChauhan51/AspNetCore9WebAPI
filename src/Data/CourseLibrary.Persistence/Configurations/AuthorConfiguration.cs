using CourseLibrary.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace CourseLibrary.Persistence.Configurations;


public sealed class AuthorConfiguration : IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> builder)
    {
        builder.Property(a => a.Id).IsRequired();
        builder.Property(a => a.FirstName).IsRequired().HasMaxLength(50);
        builder.Property(a => a.LastName).IsRequired().HasMaxLength(50);
        builder.HasKey(a => a.Id);
        builder.Property(a => a.DateOfBirth).IsRequired();
        builder.Property(a => a.MainCategory).IsRequired().HasMaxLength(50);
        builder.Navigation(a => a.Courses);
        builder.ToTable("Authors");
    }
}
