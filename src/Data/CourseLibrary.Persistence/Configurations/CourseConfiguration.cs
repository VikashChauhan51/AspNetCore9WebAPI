using CourseLibrary.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseLibrary.Persistence.Configurations;

public sealed class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.Property(c => c.Id).IsRequired();
        builder.Property(c => c.Title).IsRequired().HasMaxLength(50);
        builder.Property(c => c.Description).HasMaxLength(300);
        builder.HasKey(c => c.Id);
        builder.Property(c => c.AuthorId).IsRequired();
        builder.HasOne(c => c.Author).WithMany(c => c.Courses).HasForeignKey(c => c.AuthorId).IsRequired();
        builder.Navigation(c => c.Author);
        builder.HasIndex(c => c.AuthorId);
        builder.ToTable("Courses");
    }
}