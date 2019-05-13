using System;

namespace POC.DbSwitcher.Console.Query
{
    public class QueryTester
    {
        private readonly IDatabaseConnectionManager _connectionManager;

        public QueryTester(IDatabaseConnectionManager connectionManager)
        {
            _connectionManager = connectionManager ?? throw new ArgumentNullException(nameof(connectionManager));
        }

        public void Run()
        {
            Logger.WriteSectionHeader($"{_connectionManager.DatabaseType} ADO.NET");
            GetAndWriteQueryResult(new AdoNetMapper(_connectionManager));

            Logger.WriteSectionHeader($"{_connectionManager.DatabaseType} Dapper");
            GetAndWriteQueryResult(new DapperMapper(_connectionManager));

            void GetAndWriteQueryResult(IQuery query)
            {
                foreach (var record in query.Get(SharedConstants.UniqueIdOne))
                {
                    Logger.WriteLine(record.ToString());
                }
            }
        }
    }
}
