using DastgyrAPI.Entity;
using DastgyrAPI.Models.ViewModels.ResponseModels;

namespace WebApi.Models
{
    public class AuthenticateResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }


        public AuthenticateResponse(LoginResponse user, string token)
        {
            Id = user.Id;
            Name = user.Name;
            Email = user.Email;
            Mobile = user.Mobile;
            Address = user.Address;
            Token = token;

        }
    }
}