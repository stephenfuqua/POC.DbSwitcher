using System.Collections.Generic;
using CommandLine;
using CommandLine.Text;
using POC.DbSwitcher.Console.Migrations;

namespace POC.DbSwitcher
{
    public enum DatabaseType
    {
        Postgres,
        SqlServer
    }

    public class Options
    {
        private const string SampleConnectionString = "Server=yourServer;Initial Catalog=yourODSdbName;Integrated Security=SSPI";

        [Option('p', "provider", Required = true, HelpText ="Database provider type")]
        public DatabaseType DatabaseType { get; set; }

        [Option('t', "timeOut", Default = 60, HelpText = "Connection timeout in seconds")]
        public int TimeoutInSeconds { get; set; }

        [Option('c', "connectionString", Required = true, HelpText = "Connection string")]
        public string ConnectionString { get; set; }


        [Option('j', "journalTable", Default = "PocJournal", HelpText = "Table name for journaling table")]
        public string JournalTable { get; set; }

        [Usage(ApplicationAlias = "POC.DbSwitcher")]
        public static IEnumerable<Example> Examples
        {
            get
            {
                yield return new Example("Minimal arguments", new Options
                {
                    ConnectionString = SampleConnectionString,
                    DatabaseType = DatabaseType.SqlServer
                });
                yield return new Example("All arguments", new Options
                {
                    ConnectionString = SampleConnectionString,
                    DatabaseType = DatabaseType.SqlServer,
                    TimeoutInSeconds = 360,
                    JournalTable = "PocJournal"
                });
            }
        }

        public MigrationConfig BuildMigrationConfig()
        {
            return new MigrationConfig
            {
                ConnectionString = ConnectionString,
                JournalingTable = JournalTable,
                Timeout = TimeoutInSeconds
            };
        }
    }
}
