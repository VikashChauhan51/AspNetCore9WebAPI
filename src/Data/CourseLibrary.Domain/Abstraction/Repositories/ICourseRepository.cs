using CourseLibrary.Domain.Entities;

namespace CourseLibrary.Domain.Abstraction.Repositories;

public interface ICourseRepository 
{
    void AddCourse(Guid authorId, Course course);
    void DeleteCourse(Course course);
    Task<Course?> GetCourseAsync(Guid courseId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Course>> GetCoursesAsync(Guid authorId, CancellationToken cancellationToken = default);
    void UpdateCourse(Course course);
    Task<IEnumerable<Course>> GetCoursesAsync(CancellationToken cancellationToken = default);
    Task<Course?> GetCourseAsync(Guid authorId, Guid courseId, CancellationToken cancellationToken = default);
}