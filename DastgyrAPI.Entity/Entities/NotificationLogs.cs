using DastgyrAPI.Models.DomainModels;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DastgyrAPI.Entity
{

    [Table("notification_logs", Schema = "public")]
    public partial class NotificationLogs:EntityBase
    {
        [Column("user_id")]
        public int? UserId { get; set; }
        [Column("notification_id")]
        public int? Notification_Id { get; set; }
        [Column("push_token")]
        public string PushToken { get; set; }
        [Column("device_id")]
        public string DeviceId { get; set; }
        [Column("deleted_at")]
        public DateTime? DeletedDate { get; set; }
    }
}
