using BuildingBlocks.Module;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ResourceModule.Host;

public sealed class ResourceModuleHost : IModule, IHaveService, IHaveEndpoint
{
    public string ModuleName => "Resource";

    public IMvcBuilder ConfigureEndpoints(IMvcBuilder mvcBuilder)
    {
        mvcBuilder.AddApplicationPart(typeof(ResourceModuleHost).Assembly);
        return mvcBuilder;
    }

    public IServiceCollection ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        return services;
    }
}