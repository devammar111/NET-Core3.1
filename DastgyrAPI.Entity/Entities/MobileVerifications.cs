using DastgyrAPI.Models.DomainModels;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DastgyrAPI.Entity
{
    [Table("mobile_verifications", Schema = "public")]
    public class MobileVerifications
    {
        [Column("mobile")]
        [Key]
        public string Mobile { get; set; }
        [Column("code")]
        public string Code { get; set; }
        [Column("count")]
        public int? Count { get; set; }
        [Column("is_avail")]
        public bool? IsAvail { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [Column("created_at")]
        public DateTime CreatedDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [Column("updated_at")]
        public DateTime ModifiedDate { get; set; }
    }
}
