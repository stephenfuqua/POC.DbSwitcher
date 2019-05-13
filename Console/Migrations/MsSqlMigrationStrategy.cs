using DbUp.Builder;

namespace POC.DbSwitcher.Console.Migrations
{
    public class MsSqlMigrationStrategy : IMigrationStrategy
    {
        public string DatabaseSpecificFolderName { get; } = "MSSQL";

        public string DefaultSchema { get; } = "dbo";
        public UpgradeEngineBuilder DeployTo(SupportedDatabases supported, MigrationConfig config)
        {
            return supported
                .SqlDatabase(config.ConnectionString)
                .JournalToSqlTable(DefaultSchema, config.JournalingTable);
        }

    }
}
