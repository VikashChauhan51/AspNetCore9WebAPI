var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.CourseLibrary_API>("CourseLibrary");
builder.Build().Run();
