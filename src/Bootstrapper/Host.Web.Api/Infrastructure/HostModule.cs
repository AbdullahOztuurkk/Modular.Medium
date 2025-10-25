namespace Host.Web.Api.Infrastructure;

public sealed class HostModule : IModule, IHaveHealthCheck, IHaveService
{
    public string ModuleName => "Host";

    public IHealthChecksBuilder CheckStatus(IHealthChecksBuilder builder, IConfiguration configurationManager)
    {
        return builder.AddApplicationStatus("Application Status");
    }

    public IServiceCollection ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();

        var connectionString = configuration.GetConnectionString(ConfigurationKeys.ConnectionStrings.HealthCheckDb);
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("HealthCheckDb connection string is not configured.");
        }

        var baseUrl = configuration[ConfigurationKeys.BaseUrl];

        services.AddHealthChecksUI(opt =>
        {
            opt.AddHealthCheckEndpoint("Modular Medium", $"{baseUrl}/healthz");
            opt.SetEvaluationTimeInSeconds(60);
        })
        .AddSqlServerStorage(connectionString, (optionsBuilder) =>
        {
            optionsBuilder.ConfigureWarnings(builder => builder.Ignore(RelationalEventId.MultipleCollectionIncludeWarning));
            optionsBuilder.EnableSensitiveDataLogging(false);
            optionsBuilder.EnableDetailedErrors(false);
        });

        return services;
    }
}