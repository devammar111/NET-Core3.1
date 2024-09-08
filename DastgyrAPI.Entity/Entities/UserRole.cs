using DastgyrAPI.Models.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DastgyrAPI.Entity
{
    [Table("user_user_roles", Schema = "public")]
    public class UserRole
    {
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("user_role_id")]
        public int UserRoleId { get; set; }
    }
}
