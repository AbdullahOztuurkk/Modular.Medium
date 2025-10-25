using BuildingBlocks.Domain.Constant;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Module;

/// <summary>
/// Provides utility methods to register services, configure endpoints, 
/// and handle database migrations for modular components in an ASP.NET Core application.
/// </summary>
public static class ModuleLoader
{
    /// <summary>
    /// Registers services and endpoints for a list of modules.
    /// Adds controllers to the application and invokes module-specific service and endpoint registrations.
    /// </summary>
    /// <param name="builder">The WebApplicationBuilder used to configure the application.</param>
    /// <param name="modules">The list of modules to register.</param>
    public static WebApplicationBuilder RegisterModules(this WebApplicationBuilder builder, List<IModule> modules)
    {
        var mvcBuilder = builder.Services.AddControllers();

        var healthCheckBuilder = builder.Services.AddHealthChecks();

        var logger = LoggerFactory.Create(loggingBuilder =>
        {
            loggingBuilder.AddConsole();
        }).CreateLogger(nameof(ModuleLoader));

        foreach (var module in modules)
        {
            if (module is IHaveService serviceModule)
            {
                serviceModule.ConfigureServices(builder.Services, builder.Configuration);
            }

            if (module is IHaveEndpoint endpointModule)
            {
                endpointModule.ConfigureEndpoints(mvcBuilder);
            }

            if (module is IHaveHealthCheck healthCheckModule)
            {
                healthCheckBuilder = healthCheckModule.CheckStatus(healthCheckBuilder, builder.Configuration);
            }

            logger.LogInformation($"{module.ModuleName} Module has been registered.");
        }

        return builder;
    }

    /// <summary>
    /// Asynchronously loads and initializes application modules based on their capabilities.
    /// 
    /// This method iterates through the provided list of modules and conditionally executes
    /// operations based on implemented marker interfaces:
    /// - <see cref="IHaveSeeder"/>: Executes database seed logic.
    /// - <see cref="IHaveMigration"/>: Applies any pending database migrations.
    /// 
    /// This pattern ensures that module-specific bootstrapping logic is encapsulated and executed
    /// during application startup in a decoupled and extensible manner.
    /// </summary>
    /// <param name="app">The web application instance.</param>
    /// <param name="modules">The list of modules to load and initialize.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static async Task LoadModulesAsync(this WebApplication app, List<IModule> modules)
    {
        var seedEnabled = bool.TryParse(app.Configuration[ConfigurationKeys.SeedEnabled], out _);
        var migrationEnabled = bool.TryParse(app.Configuration[ConfigurationKeys.MigrationEnabled], out _);

        if(!seedEnabled || !migrationEnabled)
        {
            app.Lifetime.ApplicationStarted.Register(() =>
            {
                var logger = app.Services.GetRequiredService<ILoggerFactory>()
                    .CreateLogger(nameof(ModuleLoader));

                logger.LogInformation("Loading Modules part has been skipped: Seed or migration not enabled.");
            });

            return;
        }

        using var scope = app.Services.CreateScope();
        var serviceProvider = scope.ServiceProvider;

        var seederModules = modules.OfType<IHaveSeeder>();
        var migrationModules = modules.OfType<IHaveMigration>();

        if (seedEnabled)
        {
            foreach (var seeder in seederModules)
            {
                await seeder.SeedAsync(serviceProvider);
            }
        }

        if (migrationEnabled)
        {
            foreach (var migration in migrationModules)
            {
                await migration.MigrateDatabase(app);
            }
        }
    }
}