﻿using Microsoft.EntityFrameworkCore;

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
    }
}
