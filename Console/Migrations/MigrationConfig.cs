namespace POC.DbSwitcher.Console.Migrations
{
    public class MigrationConfig
    {
        public string ConnectionString { get; set; }

        public string JournalingTable { get; set; }

        public int Timeout { get; set; }
    }
}
