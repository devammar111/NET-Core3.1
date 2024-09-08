using DastgyrAPI.Models.DomainModels;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DastgyrAPI.Entity
{
    [Table("user_fcm_tokens", Schema = "public")]
    public partial class UserFcmTokens 
    {
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("fcm_token")]
        public string FcmToken { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [Column("created_at")]
        public DateTime CreatedDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [Column("updated_at")]
        public DateTime ModifiedDate { get; set; }
    }
}
