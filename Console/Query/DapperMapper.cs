using System;
using System.Collections.Generic;
using Dapper;

namespace POC.DbSwitcher.Console.Query
{
    public class DapperMapper : IQuery
    {
        private readonly IDatabaseConnectionManager _connectionManager;

        public DapperMapper(IDatabaseConnectionManager connectionManager)
        {
            _connectionManager = connectionManager ?? throw new ArgumentNullException(nameof(connectionManager));
        }

        public IEnumerable<Models.DbSwitcher> Get(Guid uniqueId)
        {
            using (var connection = _connectionManager.CreateAndOpenConnection())
            {
                return connection.Query<Models.DbSwitcher>(SharedConstants.Query, new { UniqueId = uniqueId });
            }
        }
    }
}
