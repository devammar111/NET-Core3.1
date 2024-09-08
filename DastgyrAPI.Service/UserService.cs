using DastgyrAPI.Entity;
using DastgyrAPI.Interfaces.Repositories;
using DastgyrAPI.Models.ViewModels.RequestModels;
using DastgyrAPI.Models.ViewModels.ResponseModels;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApi.Helpers;
using WebApi.Models;

namespace DastgyrAPI.Service
{
    public interface IUserService
    {
        Task<AuthenticateResponse> Authenticate(LoginRequest model);
        Task<AuthenticateResponse> AuthenticateWithOTP(string mobile, string code);
        Task<LoginResponse> CheckUserWithRole(string mobile);
        Users GetById(int id);
        Task<int> CreateMobileVerificationRecord(string mobile, string code);
        Task<bool> GetUserByMobile(string mobile);
        Task<bool> AddFcmToken(string fcmToken);

    }

    public class UserService : IUserService
    {
        private IUsersRepository _usersRepository;
        private readonly SecretSettings _appSettings;

        public UserService(IOptions<SecretSettings> appSettings, IUsersRepository usersRepository)
        {
            _appSettings = appSettings.Value;
            _usersRepository = usersRepository;
        }
        public Users GetById(int id)
        {
            return _usersRepository.GetUserByJwtId(id);
        }
        public async Task<AuthenticateResponse> Authenticate(LoginRequest model)
        {
            var user = await _usersRepository.GetUserByEmailOrMobile(model);

            // return null if user not found
            if (user == null) return null;

            // authentication successful so generate jwt token
            var token = generateJwtToken(user);

            return new AuthenticateResponse(user, token);
        }
        public async Task<AuthenticateResponse> AuthenticateWithOTP(string mobile, string code)
        {
            var user = await _usersRepository.VerifyMobileVerificationRecord(mobile, code);

            // return null if user not found
            if (user == null) return null;

            // authentication successful so generate jwt token
            var token = generateJwtToken(user);

            return new AuthenticateResponse(user, token);
        }

        public async Task<LoginResponse> CheckUserWithRole(string mobile)
        {
            var response = await _usersRepository.CheckUserWithRole(mobile);
            return response;
        }
        // helper methods

        private string generateJwtToken(LoginResponse user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<int> CreateMobileVerificationRecord(string mobile, string code)
        {
            return await _usersRepository.CreateMobileVerificationRecord(mobile, code);
        }

        public async Task<bool> GetUserByMobile(string mobile)
        {
            var user = await _usersRepository.GetUserByPhoneNumber(mobile);
            if (user == null)
            {
                return false;
            }
            return true;

        }
        public async Task<bool> AddFcmToken(string fcmToken)
        {
            return await _usersRepository.AddFcmToken(fcmToken);
        }
    }
}