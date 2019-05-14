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
            var assemblies = new []
            {
                Assembly.GetAssembly(typeof(POC.DbSwitcher.Database1.Marker)),
                Assembly.GetAssembly(typeof(POC.DbSwitcher.Database2.Marker))
            };

            var upgradeEngineBuilder = _migrationStrategy
                .DeployTo(DeployChanges.To, config)
                .WithScriptsEmbeddedInAssemblies(assemblies, filter => IsStructureFolder(filter) || IsDataFolder(filter));

            config.ExtraScriptPaths
                .ToList()
                .ForEach(x => upgradeEngineBuilder.WithScriptsFromFileSystem(x));

            var result = upgradeEngineBuilder
                .WithExecutionTimeout(TimeSpan.FromSeconds(config.Timeout))
                .WithTransaction()
                .LogToConsole()
                .Build()
                .PerformUpgrade();

            if (!result.Successful)
            {
                throw result.Error;
            }


            bool IsStructureFolder(string filter)
            {
                return filter.Contains($"Structure.{_migrationStrategy.DatabaseSpecificFolderName}.");
            }
            bool IsDataFolder(string filter)
            {
                return filter.Contains($"Data.{_migrationStrategy.DatabaseSpecificFolderName}.");
            }
        }
    }
}
