namespace CourseLibrary.Models.Dtos;

public record AuthorDto(
    Guid Id,
    string FirstName,
    string LastName,
    DateTime DateOfBirth,
    string MainCategory
);
