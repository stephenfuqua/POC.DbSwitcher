using Microsoft.EntityFrameworkCore;
using System;

namespace POC.DbSwitcher.CRUD.EntityFramework
{
    
    public abstract class PocDbSwitcherContext : DbContext
    {
        protected readonly string ConnectionString;

        public DbSet<Models.DbSwitcher> DbSwitchers { get; set; }

        public DbSet<Models.DependentTable> DependentTables { get; set; }

        protected PocDbSwitcherContext(string connectionString)
        {
            ConnectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }


    }
}
