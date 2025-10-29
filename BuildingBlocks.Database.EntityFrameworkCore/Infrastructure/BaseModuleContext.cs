using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BuildingBlocks.Database.EntityFrameworkCore.Infrastructure;

public abstract class BaseModuleContext : DbContext
{
    public static IConfiguration? Configuration { get; private set; }

    public BaseModuleContext(DbContextOptions options) : base(options)
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
}