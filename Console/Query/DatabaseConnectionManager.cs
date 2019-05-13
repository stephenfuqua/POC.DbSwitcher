using System;
using System.Data;
using System.Data.Common;

namespace POC.DbSwitcher.Console.Query
{
    public class DatabaseConnectionManager : IDatabaseConnectionManager
    {
        private readonly string _connectionStringSetting;
        private readonly DbProviderFactory _providerFactory;

        public DatabaseType DatabaseType { get; }

        public DatabaseConnectionManager(DatabaseType databaseType, DbProviderFactory providerFactory, string connectionString)
        {
            DatabaseType = databaseType;
            _providerFactory = providerFactory ?? throw new ArgumentNullException(nameof(providerFactory));
            _connectionStringSetting = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public IDbConnection CreateAndOpenConnection()
        {           
            var connection = _providerFactory.CreateConnection();
            if (connection == null)
            {
                throw new InvalidOperationException("DbProviderFactory created a null database connection.");
            }

            connection.ConnectionString = _connectionStringSetting;
            connection.Open();

            return connection;
        }

        public static DatabaseConnectionManager CreateForPostgreSQL(string connectionString)
        {
            return new DatabaseConnectionManager(DatabaseType.Postgres, Npgsql.NpgsqlFactory.Instance, connectionString);
        }

        public static DatabaseConnectionManager CreateForSqlServer(string connectionString)
        {
            return new DatabaseConnectionManager(DatabaseType.SqlServer,System.Data.SqlClient.SqlClientFactory.Instance, connectionString);
        }
    }
}
