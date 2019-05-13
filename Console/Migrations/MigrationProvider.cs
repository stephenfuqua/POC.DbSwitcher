using System;
using System.Reflection;
using DbUp;

namespace POC.DbSwitcher.Console.Migrations
{
    public class MigrationProvider
    {
        private readonly IMigrationStrategy _migrationStrategy;

        public MigrationProvider(IMigrationStrategy migrationStrategy)
        {
            _migrationStrategy = migrationStrategy;
        }

        public void Migrate(MigrationConfig config)
        {
            DeployScripts("Structure");
            DeployScripts("Data");

            void DeployScripts(string queryType)
            {
                var database1Assembly = Assembly.GetAssembly(typeof(POC.DbSwitcher.Console.Database1.Marker));

                var result = _migrationStrategy
                    .DeployTo(DeployChanges.To, config)
                    .WithScriptsEmbeddedInAssembly(database1Assembly, filter => filter.Contains($"{queryType}.{_migrationStrategy.DatabaseSpecificFolderName}."))
                    .WithExecutionTimeout(TimeSpan.FromSeconds(config.Timeout))
                    .WithTransaction()
                    .LogToConsole()
                    .Build()
                    .PerformUpgrade();

                if (!result.Successful)
                {
                    throw result.Error;
                }
            }
        }
    }
}
