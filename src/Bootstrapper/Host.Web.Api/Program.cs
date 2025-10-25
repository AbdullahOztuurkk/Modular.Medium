var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();

#if DEBUG
builder.Configuration.AddJsonFile("appsettings.Debug.json", optional: true, reloadOnChange: true);
builder.Logging.AddDebug();
#endif

#region Module Definitions
List<IModule> Modules = 
[
    new HostModule(),
    new ResourceModuleHost(),
    new InteractionModuleHost(),
    new UserModuleHost(),
    new NotificationModuleHost(),
];
#endregion

builder.RegisterModules(Modules);

builder.Services.AddOpenApi();

var app = builder.Build();

if (!app.Environment.IsProduction())
{
    app.MapOpenApi();
    app.ConfigureScalarApi();
}

app.UseHttpsRedirection()
    .UseCustomHealthChecks()
    .UseHealthCheckDashboard();

await app.LoadModulesAsync(Modules);

app.Run();
