using BuildingBlocks.Module;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InteractionModule.Host;

public sealed class InteractionModuleHost : IModule, IHaveService, IHaveEndpoint
{
    public string ModuleName => "Interaction";

    public IMvcBuilder ConfigureEndpoints(IMvcBuilder mvcBuilder)
    {
        mvcBuilder.AddApplicationPart(typeof(InteractionModuleHost).Assembly);
        return mvcBuilder;
    }

    public IServiceCollection ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        return services;
    }
}