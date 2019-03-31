using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POC.DbSwitcher.CRUD.Models
{
    [Table("DependentTable", Schema = "edfi")]
    public class DependentTable : IHaveIdProperty
    {
        [Key]
        public virtual int Id { get; set; }

        public virtual int DbSwitcherId { get; set; }

        public virtual DateTime CreatedDate { get; set; }
    }
}
