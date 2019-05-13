using System.Data;

namespace POC.DbSwitcher.Console.Query
{
    public interface IDatabaseConnectionManager
    {
        DatabaseType DatabaseType { get; }
        IDbConnection CreateAndOpenConnection();
    }
}
