using Microsoft.EntityFrameworkCore;

namespace POC.DbSwitcher.CRUD.EntityFramework
{
    public class PgSqlContext : PocDbSwitcherContext
    {
        public PgSqlContext(string connectionString) : base(connectionString)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // Map Postgres smallint to .NET boolean
            modelBuilder.Entity<Models.DbSwitcher>()
                .Property(t => t.IsTrue)
                .HasConversion(
                    v => v ? 1 : 0,
                    v => v == 1
                );

        }
    }
}
