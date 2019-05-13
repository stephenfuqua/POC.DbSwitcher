using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POC.DbSwitcher.Console.Models
{
    [Table(DbSwitcher.Constants.TableName, Schema = DbSwitcher.Constants.SchemaName)]
    public class DbSwitcher : IHaveIdProperty
    {
        public class Constants
        {
            public const string TableName = "DbSwitcher";
            public const string SchemaName = "edfi";

            public class ColumnNames
            {
                public const string Id = "Id";
                public const string Summary = "Summary";
                public const string CreatedDate = "CreatedDate";
                public const string UniqueId = "UniqueId";
                public const string IsTrue = "IsTrue";
            }
        }

        [Key]
        public virtual int Id { get; set; }

        [MaxLength(50)]
        public virtual string Summary { get; set; }

        public virtual DateTime CreatedDate { get; set; }

        public virtual Guid UniqueId { get; set; }

        public virtual bool IsTrue { get; set; }


        public override string ToString()
        {
            return $"Summary: {Summary}, CreatedDate: {CreatedDate}, UniqueId: {UniqueId}, IsTrue: {IsTrue}";
        }

    }
}
