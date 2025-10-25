using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Module;

/// <summary>
/// Marker interface for a module
/// </summary>
public interface IModule
{
    /// <summary>
    /// Gets or sets the name of the module associated with this instance.
    /// </summary>
    public string ModuleName { get; }
}

/// <summary>
/// Marker interface for modules that require service registration.
/// Modules implementing this interface will expose a method to register their dependencies and services
/// into the application's Inversion of Control (IoC) container, typically during startup.
/// </summary>
public interface IHaveService
{
    public IServiceCollection ConfigureServices(IServiceCollection services, IConfiguration configuration);
}

/// <summary>
/// Marker interface for modules that expose API endpoints or MVC routes.
/// Modules implementing this interface will register their specific controllers or endpoint definitions
/// with the application's routing system (e.g., ASP.NET Core MVC or Minimal APIs).
/// </summary>
public interface IHaveEndpoint
{
    IMvcBuilder ConfigureEndpoints(IMvcBuilder mvcBuilder);
}

/// <summary>
/// Marker interface for modules responsible for database migrations.
/// Modules implementing this interface manage their own database schema changes and
/// will include a method to apply pending migrations during application startup or a specific lifecycle event.
/// </summary>
public interface IHaveMigration
{
    Task<WebApplication> MigrateDatabase(WebApplication app);
}

/// <summary>
/// Marker interface for modules that contain seeding logic for initial data.
/// Modules implementing this interface are responsible for populating their respective data stores
/// with default or sample data, typically executed once during application initialization or deployment.
/// </summary>
public interface IHaveSeeder
{
    Task SeedAsync(IServiceProvider serviceProvider);
}

/// <summary>
/// Marker interface for modules that provide health check functionalities.
/// Modules implementing this interface expose specific health checks to monitor
/// the operational status and readiness of their components within the application.
/// </summary>
public interface IHaveHealthCheck
{
    IHealthChecksBuilder CheckStatus(IHealthChecksBuilder builder, IConfiguration configurationManager);
}