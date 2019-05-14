using System;
using System.Data;
using System.Data.Common;

namespace POC.DbSwitcher.Console.Query
{
    /*
     * This class is distinct from the ConnectionFactory because it is generalizable / useful outside of this application,
     * and it is specific to one function: providing an open database connection utilizing DbProviderFactory. The other
     * class is a general factory for this application, replacing a DI container.
     */

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

        /*
         * In a .NET Framework application with an app.config or web.config file, the provider factories would be setup in the
         * config file and the connection strings would have providerName properties on them. .NET Core does not appear to have 
         * support for switching providers solely through config file, although my search was not as exhaustive as it could be.
         */
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
