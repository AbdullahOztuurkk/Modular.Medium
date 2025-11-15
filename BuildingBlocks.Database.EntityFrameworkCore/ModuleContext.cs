using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BuildingBlocks.Database.EntityFrameworkCore;

public abstract class ModuleContext : DbContextd
{
    public static IConfiguration? Configuration { get; private set; }
    public abstract string SchemaName { get; }

    public ModuleContext(DbContextOptions options) : base(options)
    {
        string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
        Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
#if DEBUG
                .AddJsonFile($"appsettings.Local.json", optional: true)
#endif
                .AddEnvironmentVariables()
                .Build();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(SchemaName);
        base.OnModelCreating(modelBuilder);
    }
}