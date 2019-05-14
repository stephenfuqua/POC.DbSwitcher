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
                var switcher = new Dictionary<DatabaseType, IConnectionFactory>
                {
                    { DatabaseType.Postgres, new PostgreSqlConnectionFactory(options.ConnectionString) },
                    { DatabaseType.SqlServer, new SqlServerConnectionFactory(options.ConnectionString) }
                };

                var connectionFactory = switcher[options.DatabaseType];

                try
                {
                    Logger.WriteSectionHeader("Database Migration using DbUp");
                    new MigrationProvider(connectionFactory.MigrationStrategy)
                        .Migrate(options.BuildMigrationConfig());

                    var tester = new CrudTester(connectionFactory.CreateEntityFrameworkContext(), connectionFactory.CreateNHibernateRepository());

                    Logger.WriteSectionHeader("Entity Framework CRUD Tests");
                    tester.RunEntityFrameworkTests();

                    Logger.WriteSectionHeader("NHibernate CRUD Tests");
                    tester.RunNHibernateTests();

                    Logger.WriteSectionHeader("ADO.NET vs Dapper");
                    new QueryTester(connectionFactory.CreateConnectionManager()).Run();

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
