namespace CourseLibrary.Logging.Telemetry.Enrichments;

using OpenTelemetry;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;

public class CloudEnrichmentProcessor : BaseProcessor<Activity>
{
    private readonly string _provider;
    private readonly IConfiguration _configuration;

    public CloudEnrichmentProcessor(IConfiguration configuration)
    {
        _provider = configuration["Cloud:Provider"]?.ToLowerInvariant() ?? "unknown";
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    public override void OnStart(Activity activity)
    {
        if (activity == null)
            return;

        switch (_provider)
        {
            case "azure":
                activity.SetTag("cloud.provider", "azure");
                activity.SetTag("cloud.region", _configuration["Azure:Region"] ?? "unknown");
                activity.SetTag("host.name", Environment.MachineName);
                activity.SetTag("app.service.name", _configuration["Azure:AppServiceName"] ?? "unknown");
                break;

            case "aws":
                activity.SetTag("cloud.provider", "aws");
                activity.SetTag("cloud.region", _configuration["AWS:Region"] ?? "unknown");
                activity.SetTag("cloud.account.id", _configuration["AWS:AccountId"] ?? "unknown");
                activity.SetTag("ecs.task.arn", _configuration["AWS:EcsTaskArn"] ?? "unknown");
                break;

            default:
                activity.SetTag("cloud.provider", "unknown");
                break;
        }
    }
}


