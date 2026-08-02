
await WebApplication.CreateBuilder(args)
  .ConfigureServices()
  .ConfigurePipelines()
  .RunAsync();