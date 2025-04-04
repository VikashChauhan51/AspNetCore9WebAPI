namespace CourseLibrary.Models.Dtos;

public record AuthorWithCoursesDto(
    Guid Id,
    string FirstName,
    string LastName,
    DateTime DateOfBirth,
    string MainCategory,
    List<CourseDto> Courses
);
