using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POC.DbSwitcher.CRUD.Models
{
    [Table("DbSwitcher", Schema = "edfi")]
    public class DbSwitcher  : IHaveIdProperty
    {
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
