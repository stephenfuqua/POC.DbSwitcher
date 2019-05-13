using System;
using System.Collections.Generic;
using CommandLine;
using POC.DbSwitcher.Console.Migrations;
using POC.DbSwitcher.Console.CRUD;
using POC.DbSwitcher.Console.Query;

namespace POC.DbSwitcher.Console
{
    class Program
    {
        public interface IDatabaseStrategy
        {
            IMigrationStrategy MigrationStrategy { get; }
            IDatabaseConnectionManager CreateConnectionManager(string connectionString);
        }

        public class SqlServerStrategy : IDatabaseStrategy
        {
            public IMigrationStrategy MigrationStrategy { get; } = new MsSqlMigrationStrategy();

            public IDatabaseConnectionManager CreateConnectionManager(string connectionString) => DatabaseConnectionManager.CreateForSqlServer(connectionString);
        }

        public class PostgreSQLStrategy : IDatabaseStrategy
        {
            public IMigrationStrategy MigrationStrategy { get; } = new PgSqlMigrationStrategy();

            public IDatabaseConnectionManager CreateConnectionManager(string connectionString) => DatabaseConnectionManager.CreateForPostgreSQL(connectionString);
        }

        static void Main(string[] args)
        {

            new Parser(config =>
            {
                config.CaseInsensitiveEnumValues = true;
                config.HelpWriter = System.Console.Error;
            })
                .ParseArguments<Options>(args)
                .WithParsed(RunWithOptions)
                .WithNotParsed(WriteArgumentErrorMessage);


            void RunWithOptions(Options options)
            {
                var switcher = new Dictionary<DatabaseType, IDatabaseStrategy>
                {
                    { DatabaseType.Postgres, new PostgreSQLStrategy() },
                    { DatabaseType.SqlServer, new SqlServerStrategy() }
                };

                var strategy = switcher[options.DatabaseType];

                try
                {

                    Logger.WriteSectionHeader("Database Migration using DbUp");
                    new MigrationProvider(strategy.MigrationStrategy)
                        .Migrate(options.BuildMigrationConfig());

                    var tester = new CrudTester(options.DatabaseType, options.ConnectionString);

                    Logger.WriteSectionHeader("Entity Framework CRUD Tests");
                    tester.RunEntityFrameworkTests();

                    Logger.WriteSectionHeader("NHibernate CRUD Tests");
                    tester.RunNHibernateTests();

                    Logger.WriteSectionHeader("ADO.NET vs Dapper");
                    new QueryTester(strategy.CreateConnectionManager(options.ConnectionString)).Run();

                    Exit(0);
                }
                catch (Exception ex)
                {
                    Logger.ErrorLine(ex.Message);
                    Exit(-2);
                }

            }

            void WriteArgumentErrorMessage(IEnumerable<Error> errors)
            {
                Exit(-1);
            }

            void Exit(int code)
            {
                Environment.ExitCode = code;
                System.Console.Write("Press any key to continue.");
                System.Console.ReadKey();
            }

        }
    }
}
