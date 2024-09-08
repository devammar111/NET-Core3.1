using System.ComponentModel.DataAnnotations;

namespace DastgyrAPI.Models.ViewModels.RequestModels
{
    public class LoginRequest
    {
        [Required]
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool? IsTwilio { get; set; } = false;
    }
    public class LoginRequestOTP
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string OTP { get; set; }
    }
}
