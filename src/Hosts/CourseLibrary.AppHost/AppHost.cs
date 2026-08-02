var builder = DistributedApplication.CreateBuilder(args);

var api = builder.AddProject<Projects.CourseLibrary_Api>("courselibrary-api");

builder.Build().Run();
