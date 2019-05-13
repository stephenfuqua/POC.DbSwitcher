using System.Data;

namespace POC.DbSwitcher.Query
{
    public interface IDatabaseConnectionManager
    {
        DatabaseType DatabaseType { get; }
        IDbConnection CreateAndOpenConnection();
    }
}
