using BuildingBlocks.Module;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace NotificationModule.Host;

public sealed class NotificationModuleHost : IModule, IHaveService, IHaveEndpoint
{
    public string ModuleName => "Notification";

    public IMvcBuilder ConfigureEndpoints(IMvcBuilder mvcBuilder)
    {
        mvcBuilder.AddApplicationPart(typeof(NotificationModuleHost).Assembly);
        return mvcBuilder;
    }

    public IServiceCollection ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        return services;
    }
}