using DastgyrAPI.Models.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DastgyrAPI.Entity
{
    [Table("users", Schema = "public")]
    public class Users : EntityBase
    {
        [Column("name")]
        public string Name { get; set; }
        public string mobile { get; set; }
        [Column("email")]
        public string Email { get; set; }
        [Column("password")]
        public string Password { get; set; }
        [Column("gender")]
        public string Gender { get; set; }
        [Column("is_active")]
        public bool? IsActive { get; set; }
        [Column("is_verify")]
        public bool? IsVerify { get; set; }
        [Column("latlng")]
        public string Latlng { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [Column("created_at")] 
        public DateTime? CreatedDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [Column("updated_at")]
        public DateTime? UpdatedDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [Column("deleted_at")]
        public DateTime? DeletedDate { get; set; }
        [Column("created_by")]
        public int? CreatedBy { get; set; }

        [Column("address")]
        public string Address { get; set; }
        [Column("refer_by")]
        public int? ReferBy { get; set; }
        [Column("refer_at")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? ReferAt { get; set; }
        [Column("ref_code")]
        public string ReferCode { get; set; }
        [Column("is_rewarded")]
        public bool? IsRewarded { get; set; }

    }
}
