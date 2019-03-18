using Microsoft.EntityFrameworkCore;

namespace POC.DbSwitcher.CRUD.EntityFramework
{
    public class MsSqlContext : PocDbSwitcherContext
    {
        public MsSqlContext(string connectionString) : base(connectionString)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionString);
        }
    }
}
