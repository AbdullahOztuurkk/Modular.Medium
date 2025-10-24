using BuildingBlocks.Module;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace UserModule.Host;

public sealed class UserModuleHost : IModule, IHaveService, IHaveEndpoint
{
    public string ModuleName => "User";

    public IMvcBuilder ConfigureEndpoints(IMvcBuilder mvcBuilder)
    {
        mvcBuilder.AddApplicationPart(typeof(UserModuleHost).Assembly);
        return mvcBuilder;
    }

    public IServiceCollection ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        return services;
    }
}