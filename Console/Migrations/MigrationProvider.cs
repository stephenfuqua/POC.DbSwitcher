﻿using DbUp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DbUp.Builder;

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

            InstallStructure(assemblies);
            InstallData(assemblies);
            InstallExtras();


            void InstallStructure(Assembly[] assembliesContainingScripts)
            {
                var upgradeEngineBuilder = _migrationStrategy
                    .DeployTo(DeployChanges.To, config)
                    .WithScriptsEmbeddedInAssemblies(assembliesContainingScripts, IsStructureFolder);

                PerformUpgrade(upgradeEngineBuilder);
            }

            void InstallData(Assembly[] assembliesContainingScripts)
            {
                var upgradeEngineBuilder = _migrationStrategy
                    .DeployTo(DeployChanges.To, config)
                    .WithScriptsEmbeddedInAssemblies(assembliesContainingScripts, IsDataFolder);

                PerformUpgrade(upgradeEngineBuilder);
            }

            void InstallExtras()
            {
                if (!config.ExtraScriptPaths.Any())
                {
                    return;
                }

                // TODO: separate these scripts for structure and data directories
                var upgradeEngineBuilder = _migrationStrategy
                        .DeployTo(DeployChanges.To, config);

                config.ExtraScriptPaths
                    .ToList()
                    .ForEach(x => upgradeEngineBuilder.WithScriptsFromFileSystem(x));

                PerformUpgrade(upgradeEngineBuilder);
            }

            void PerformUpgrade(UpgradeEngineBuilder builder)
            {
                var result = builder
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
