using CourseLibrary.Domain.Entities;
using CourseLibrary.Models.Dtos;
using Mapster;

namespace CourseLibrary.Models.Mapping;

public sealed class AuthorMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Author, AuthorWithCoursesDto>()
              .Map(dest => dest.Courses, src => src.Courses.Adapt<List<CourseDto>>())
              .Compile();

        config.NewConfig<AuthorWithCoursesDto, Author>()
              .Map(dest => dest.Courses, src => src.Courses.Adapt<List<Course>>())
              .Compile();
    }
}
