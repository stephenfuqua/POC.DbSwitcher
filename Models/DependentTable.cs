using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POC.DbSwitcher.Models
{
    [Table(DependentTable.Constants.TableName, Schema = DependentTable.Constants.SchemaName)]
    public class DependentTable : IHaveIdProperty
    {
        public class Constants
        {
            public const string TableName = "DependentTable";
            public const string SchemaName = "edfi";

            public class ColumnNames
            {
                public const string Id = "Id";
                public const string DbSwitcherId = " DbSwitcherId";
                public const string CreatedDate = "CreatedDate";
            }
        }

        [Key]
        public virtual int Id { get; set; }

        public virtual int DbSwitcherId { get; set; }

        public virtual DateTime CreatedDate { get; set; }
    }
}
