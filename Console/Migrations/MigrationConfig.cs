using System.Collections.Generic;

namespace POC.DbSwitcher.Console.Migrations
{
    public class MigrationConfig
    {
        public string ConnectionString { get; set; }

        public string JournalingTable { get; set; }

        public int Timeout { get; set; }

        public IEnumerable<string> ExtraScriptPaths { get; set; }
    }
}
