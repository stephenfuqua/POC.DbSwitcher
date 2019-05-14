using DbUp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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
                DeployScripts(x, "Data");
            });

            config.ExtraScriptPaths.ToList().ForEach(DeployExtraScripts);


            void DeployScripts(Assembly assembly, string queryType)
            {
                var result = _migrationStrategy
                    .DeployTo(DeployChanges.To, config)
                    .WithScriptsEmbeddedInAssembly(assembly, filter => filter.Contains($"{queryType}.{_migrationStrategy.DatabaseSpecificFolderName}."))
                    // It might be feasible to instead use WithScriptsEmbeddedInAssemblies; didn't test it out.
                    // Doing so might be advantageous in that we would have a single transaction wrapping all
                    // migrations, rather than one transaction per assembly.
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

            void DeployExtraScripts(string path)
            {
                // As with comment above, probably better to combine into a single call
                // so that all migrations are in a single transaction.
                var result = _migrationStrategy
                    .DeployTo(DeployChanges.To, config)
                    .WithScriptsFromFileSystem(path)
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
