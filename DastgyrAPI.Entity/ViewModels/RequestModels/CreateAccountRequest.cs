using DastgyrAPI.Models.HelperModels;
using System.ComponentModel.DataAnnotations;

namespace DastgyrAPI.Entity.ViewModels.RequestModels
{
    public class CreateAccountRequest
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        public string DeviceToken { get; set; } = string.Empty;
        [Required]
        public UserTypeEnum UserType { get; set; }
    }
}
