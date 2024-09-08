using DastgyrAPI.Entity;
using DastgyrAPI.Models.ViewModels.RequestModels;
using DastgyrAPI.Models.ViewModels.ResponseModels;
using System.Threading.Tasks;

namespace DastgyrAPI.Interfaces.Repositories
{
    public interface IUsersRepository : IBaseRepository<Users>
    {
        Task<LoginResponse> GetUserByEmailOrMobile(LoginRequest model);
        Users GetUserByJwtId(int id);
        Task<LoginResponse> GetUserByPhoneNumber(string phoneNumber);
        Task<int> CreateMobileVerificationRecord(string mobile, string code);
        Task<LoginResponse> VerifyMobileVerificationRecord(string mobile, string code);
        Task<LoginResponse> CheckUserWithRole(string mobile);
        Task<bool> AddFcmToken(string fcmToken);


    }
}
