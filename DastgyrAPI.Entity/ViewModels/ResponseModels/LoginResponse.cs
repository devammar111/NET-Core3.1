
using System;

namespace DastgyrAPI.Models.ViewModels.ResponseModels
{
    public class LoginResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Gender { get; set; }
        public bool? IsActive { get; set; }
        public bool IsVerify { get; set; }
        public string Latlng { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime DeletedDate { get; set; }
        public int CreatedBy { get; set; }
        public string Address { get; set; }
        public int ReferBy { get; set; }
        public DateTime ReferAt { get; set; }
        public string ReferCode { get; set; }
        public bool IsRewarded { get; set; }
        public int? RoleId { get; set; }
        public int? Role { get; set; }
    }
    public class OTPResponse
    {
        public string Status { get; set; }
        public string Message { get; set; }
    }
}
