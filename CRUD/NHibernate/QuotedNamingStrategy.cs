using System;
using NHibernate.Cfg;

namespace POC.DbSwitcher.CRUD.NHibernate
{
    public class QuotedNamingStrategy : INamingStrategy
    {
        public string ClassToTableName(string className)
        {
            return $"\"{className}\"";
        }

        public string PropertyToColumnName(string propertyName)
        {
            return propertyName;
        }

        public string TableName(string tableName)
        {
            return $"\"{tableName}\"";
        }

        public string ColumnName(string columnName)
        {
            return $"\"{columnName}\"";
        }

        public string PropertyToTableName(string className, string propertyName)
        {
            throw new NotImplementedException();
        }

        public string LogicalColumnName(string columnName, string propertyName)
        {
            throw new NotImplementedException();
        }
    }
}
