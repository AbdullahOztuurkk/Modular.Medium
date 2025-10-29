namespace BuildingBlocks.Domain.Constant;

public sealed class ConfigurationKeys
{
    public const string SeedEnabled = "SeedEnabled";
    public const string MigrationEnabled = "MigrationEnabled";
    public const string BaseUrl = "BaseUrl";
    public const string EncryptionKey = "EncryptionKey";

    public static class ConnectionStrings
    {
        public const string HealthCheckDb = "HealthCheckDb";
    }
}
