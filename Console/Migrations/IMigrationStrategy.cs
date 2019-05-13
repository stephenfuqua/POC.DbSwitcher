using DbUp.Builder;

namespace POC.DbSwitcher.Console.Migrations
{
    public interface IMigrationStrategy
    {
        string DatabaseSpecificFolderName { get; }

        string DefaultSchema { get; }

        UpgradeEngineBuilder DeployTo(SupportedDatabases supported, MigrationConfig config);
    }
}
