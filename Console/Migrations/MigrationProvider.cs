using System;
using System.Collections.Generic;
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
            var assemblies = new List<Assembly>
            {
                Assembly.GetAssembly(typeof(POC.DbSwitcher.Database1.Marker)),
                Assembly.GetAssembly(typeof(POC.DbSwitcher.Database2.Marker))
            };

            assemblies.ForEach(x =>
            {
                DeployScripts(x, "Structure");
                DeployScripts(x,"Data");
            });

            void DeployScripts(Assembly assembly, string queryType)
            {
                var result = _migrationStrategy
                    .DeployTo(DeployChanges.To, config)
                    .WithScriptsEmbeddedInAssembly(assembly, filter => filter.Contains($"{queryType}.{_migrationStrategy.DatabaseSpecificFolderName}."))
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
