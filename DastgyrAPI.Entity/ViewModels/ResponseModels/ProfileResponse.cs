using System;

namespace DastgyrAPI.Models.ViewModels.ResponseModels
{
    public class ProfileResponse
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string RoleName { get; set; }
        public string State { get; set; }
        public string Gender { get; set; }
        public string Country { get; set; }
        public DateTime? UserCreatedDate { get; set; }
    }
}
