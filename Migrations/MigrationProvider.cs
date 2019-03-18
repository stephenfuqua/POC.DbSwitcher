using System;
using System.Reflection;
using DbUp;

namespace POC.DbSwitcher.Migrations
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
                var result = _migrationStrategy
                    .DeployTo(DeployChanges.To, config)
                    .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly(), filter => filter.Contains($"{queryType}.{_migrationStrategy.DatabaseSpecificFolderName}."))
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
