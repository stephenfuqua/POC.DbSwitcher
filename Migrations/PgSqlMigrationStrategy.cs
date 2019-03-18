using DbUp.Builder;

namespace POC.DbSwitcher.Migrations
{
    public class PgSqlMigrationStrategy : IMigrationStrategy
    {
        public string DatabaseSpecificFolderName { get; } = "PGSQL";

        public string DefaultSchema { get; } = "public";

        public UpgradeEngineBuilder DeployTo(SupportedDatabases supported, MigrationConfig config)
        {
            return supported
                .PostgresqlDatabase(config.ConnectionString)
                .JournalToPostgresqlTable(DefaultSchema, config.JournalingTable);
        }

    }
}
