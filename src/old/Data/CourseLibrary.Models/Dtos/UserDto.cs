namespace CourseLibrary.Models.Dtos;
public record UserDto(
        Guid Id,
        string FirstName,
        string LastName,
        string Email,
        string Password
    );
