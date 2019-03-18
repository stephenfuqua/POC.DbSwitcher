using CommandLine;
using POC.DbSwitcher.Migrations;
using System;
using System.Collections.Generic;
using POC.DbSwitcher.CRUD;

namespace POC.DbSwitcher
{
    class Program
    {
        static void Main(string[] args)
        {

            new Parser(config =>
            {
                config.CaseInsensitiveEnumValues = true;
                config.HelpWriter = Console.Error;
            })
                .ParseArguments<Options>(args)
                .WithParsed(RunWithOptions)
                .WithNotParsed(WriteArgumentErrorMessage);


            void RunWithOptions(Options options)
            {
                Console.WriteLine("Starting database migration....");

                var switcher = new Dictionary<DatabaseType, Func<IMigrationStrategy>>
                {
                    { DatabaseType.Postgres, ()=> new PgSqlMigrationStrategy() },
                    { DatabaseType.SqlServer, ()=> new MsSqlMigrationStrategy() }
                };

                try
                {
                    new MigrationProvider(switcher[options.DatabaseType]())
                        .Migrate(options.BuildMigrationConfig());

                    Console.WriteLine("Migration complete.");

                    Console.WriteLine("Starting CRUD tests...");

                    new CrudTester(options.DatabaseType, options.ConnectionString)
                        .RunTests();

                    Exit(0);
                }
                catch(Exception ex)
                {
                    Console.Error.WriteLine(ex.Message);
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
                Console.Write("Press any key to continue.");
                Console.ReadKey();
            }
        }
    }
}
