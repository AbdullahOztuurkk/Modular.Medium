var builder = WebApplication.CreateBuilder(args);

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

builder.Services.AddOpenApi();

var app = builder.Build();

if (!app.Environment.IsProduction())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();
