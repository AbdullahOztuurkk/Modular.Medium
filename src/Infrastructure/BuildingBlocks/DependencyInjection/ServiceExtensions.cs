using BuildingBlocks.Services.CurrentUser;
using BuildingBlocks.Services.Encryption;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.DependencyInjection;

public static class ServiceExtensions
{
    public static IServiceCollection AddInfrastructureBuildingBlocks(this IServiceCollection services)
    {
        services.AddScoped<ICurrentUser, CurrentUser>();

        services.AddScoped<IEncryptionService, EncryptionService>();

        return services;
    }
}
