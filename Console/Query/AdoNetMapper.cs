using System;
using System.Collections.Generic;
using System.Data;

namespace POC.DbSwitcher.Console.Query
{
    public class AdoNetMapper : IQuery
    {
        private readonly IDatabaseConnectionManager _connectionManager;

        public AdoNetMapper(IDatabaseConnectionManager connectionManager)
        {
            _connectionManager = connectionManager ?? throw new ArgumentNullException(nameof(connectionManager));
        }

        public IEnumerable<Models.DbSwitcher> Get(Guid uniqueId)
        {
            using (var connection = _connectionManager.CreateAndOpenConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = SharedConstants.Query;

                    var uniqueIdParameter = command.CreateParameter();
                    uniqueIdParameter.ParameterName = SharedConstants.UniqueIdParameterName;
                    uniqueIdParameter.Value = uniqueId;
                    command.Parameters.Add(uniqueIdParameter);


                    using (var reader = command.ExecuteReader())
                    {
                        var idOrdinal = reader.GetOrdinal(Models.DbSwitcher.Constants.ColumnNames.Id);
                        var summaryOrdinal = reader.GetOrdinal(Models.DbSwitcher.Constants.ColumnNames.Summary);
                        var createdDateOrdinal = reader.GetOrdinal(Models.DbSwitcher.Constants.ColumnNames.CreatedDate);
                        var isTrueOrdinal = reader.GetOrdinal(Models.DbSwitcher.Constants.ColumnNames.IsTrue);
                        var uniqueIdOrdinal = reader.GetOrdinal(Models.DbSwitcher.Constants.ColumnNames.UniqueId);

                        while (reader.Read())
                        {
                            yield return new Models.DbSwitcher
                            {
                                // TODO: IsDbNull handling
                                Id = reader.GetInt32(idOrdinal),
                                Summary = reader.GetString(summaryOrdinal),
                                CreatedDate = reader.GetDateTime(createdDateOrdinal),
                                IsTrue = reader.GetBoolean(isTrueOrdinal),
                                UniqueId = reader.GetGuid(uniqueIdOrdinal),
                            };
                        }
                    }
                }
            }
        }
    }
}
