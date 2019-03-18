using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POC.DbSwitcher.CRUD.Models
{
    [Table("DbSwitcher", Schema = "edfi")]
    public class DbSwitcher
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(50)]
        public string Summary { get; set; }
        public DateTime CreatedDate { get; set; }

        public Guid UniqueId { get; set; }

        public bool IsTrue { get; set; }

        public bool IsTrue2 { get; set; }


        public override string ToString()
        {
            return $"Summary: {Summary}, CreatedDate: {CreatedDate}, UniqueId: {UniqueId}, IsTrue: {IsTrue}, IsTrue2: {IsTrue2}";
        }

    }
}
