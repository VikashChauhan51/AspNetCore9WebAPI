namespace CourseLibrary.Domain.Entities;

public class Course
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;   

    public string? Description { get; set; }

    public Author Author { get; set; } = null!;

    public Guid AuthorId { get; set; }
}