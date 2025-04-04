using CourseLibrary.Domain.Entities;

namespace CourseLibrary.Domain.Abstraction.Repositories;

public interface ICourseRepository 
{
    void AddCourse(Guid authorId, Course course);
    void DeleteCourse(Course course);
    Task<Course?> GetCourseAsync(Guid courseId);
    Task<IEnumerable<Course>> GetCoursesAsync(Guid authorId);
    void UpdateCourse(Course course);
    Task<IEnumerable<Course>> GetCoursesAsync();
    Task<Course?> GetCourseAsync(Guid authorId, Guid courseId);
}