using POC.DbSwitcher.Console.CRUD.EntityFramework;
using POC.DbSwitcher.Console.Migrations;
using POC.DbSwitcher.Console.Query;
using System;
using POC.DbSwitcher.Console.CRUD.NHibernate;

namespace POC.DbSwitcher.Console
{
    public interface IConnectionFactory
    {
        IMigrationStrategy MigrationStrategy { get; }
        IDatabaseConnectionManager CreateConnectionManager();
        PocDbSwitcherContext CreateEntityFrameworkContext();
        PocDbSwitcherRepository CreateNHibernateRepository();
    }

    public class SqlServerConnectionFactory : IConnectionFactory
    {
        public string ConnectionString { get; }

        public SqlServerConnectionFactory(string connectionString)
        {
            ConnectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public IMigrationStrategy MigrationStrategy { get; } = new MsSqlMigrationStrategy();

        public IDatabaseConnectionManager CreateConnectionManager() => DatabaseConnectionManager.CreateForSqlServer(ConnectionString);

        public PocDbSwitcherContext CreateEntityFrameworkContext() => new MsSqlContext(ConnectionString);

        public PocDbSwitcherRepository CreateNHibernateRepository() => PocDbSwitcherRepository.CreateForSqlServer(ConnectionString);
    }

    public class PostgreSqlConnectionFactory : IConnectionFactory
    {
        public string ConnectionString { get; }

        public PostgreSqlConnectionFactory(string connectionString)
        {
            ConnectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public IMigrationStrategy MigrationStrategy { get; } = new PgSqlMigrationStrategy();

        public IDatabaseConnectionManager CreateConnectionManager() => DatabaseConnectionManager.CreateForPostgreSQL(ConnectionString);

        public PocDbSwitcherContext CreateEntityFrameworkContext() => new PgSqlContext(ConnectionString);

        public PocDbSwitcherRepository CreateNHibernateRepository() => PocDbSwitcherRepository.CreateForPostgreSQL(ConnectionString);
    }


}
