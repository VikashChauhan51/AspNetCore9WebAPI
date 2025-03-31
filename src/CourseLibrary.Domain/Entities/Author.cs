namespace CourseLibrary.Domain.Entities;

public class Author
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    public DateTime DateOfBirth { get; set; }
    public string MainCategory { get; set; } = string.Empty;

    public ICollection<Course> Courses { get; set; } = new List<Course>();
}