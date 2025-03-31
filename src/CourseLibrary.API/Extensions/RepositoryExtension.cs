using CourseLibrary.Domain.Abstraction.Repositories;
using CourseLibrary.Persistence.DbContexts;
using CourseLibrary.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CourseLibrary.API.Extensions;

public static class RepositoryExtension
{
    public static IServiceCollection CongigureRepositories(this IServiceCollection services, IConfiguration Configuration)
    {

        services.AddScoped<IAuthorRepository,
         AuthorRepository>();
        services.AddScoped<ICourseRepository,
         CourseRepository>();
        services.AddScoped<IUserRepository,
         UserRepository>();

        services.AddDbContext<CourseLibraryContext>(options =>
        {
            options.UseSqlServer(Configuration.GetConnectionString("CourseLibraryDatabase"));
    });
        return services;
    }
}
