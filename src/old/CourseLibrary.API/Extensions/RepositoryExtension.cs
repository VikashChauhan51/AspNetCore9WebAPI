using CourseLibrary.Domain.Abstraction.Repositories;
using CourseLibrary.Persistence.DbContexts;
using CourseLibrary.Persistence.Repositories;
using CourseLibrary.Resilience.Configurations;
using CourseLibrary.Resilience.Policies;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Polly;
using System.Data;

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
            options.UseSqlServer(
                Configuration.GetConnectionString("CourseLibraryDatabase"),
                sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(1),
                        errorNumbersToAdd: null
                    );
                });
        });

        services.AddScoped<IDbConnection>(sp =>
        new SqlConnection(Configuration.GetConnectionString("CourseLibraryDatabase")));

        services.AddSingleton<IAsyncPolicy>(sp =>
            PolicyFactory.CreateSqlRetryPolicy(sp.GetRequiredService<IOptions<PolyOptions>>().Value, sp.GetRequiredService<ILogger>()));

        return services;
    }
}
