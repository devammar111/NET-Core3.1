using DastgyrAPI.Models.DomainModels;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DastgyrAPI.Entity
{
    [Table("brands", Schema = "public")]
    public partial class Brands:EntityBase
    {
        [Column("name")]
        public string Name { get; set; }
        [Column("code")]
        public string Code { get; set; }
        [Column("is_active")]
        public bool? IsActive { get; set; }
        [Column("deleted_at")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime DeletedAt { get; set; }
        [Column("image")]
        public string Image { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [Column("created_at")]
        public DateTime CreatedDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [Column("updated_at")]
        public DateTime ModifiedDate { get; set; }
        [Column("created_by")]
        public int CreatedBy { get; set; }
    }
}
