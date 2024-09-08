using DastgyrAPI.Interfaces.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using DastgyrAPI.Entity;
using DastgyrAPI.Models.ViewModels.ResponseModels;
using DastgyrAPI.Models.ViewModels.RequestModels;
using System;
using AutoMapper;
using DastgyrAPI.Models.HelperModels;

namespace DastgyrAPI.Repositories
{
    public class UsersRepository : BaseRepository<Users>, IUsersRepository
    {
        #region Initialization
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        public UsersRepository(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor, IMapper mapper) : base(dbContext, httpContextAccessor)

        {
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }


        #endregion

        #region Implementation Methods
        public async Task<LoginResponse> GetUserByEmailOrMobile(LoginRequest model)
        {
            LoginResponse responseModel = new LoginResponse();
            var user = await _dbContext.Users
                                .Where(u => u.Password == model.Password && (u.Email == model.UserName || u.mobile == model.UserName))
                                .FirstOrDefaultAsync();
            if (user != null)
            {
                var currentUserRole = await _dbContext.UserRoles
                    .Where(r => r.UserId == user.Id && r.UserRoleId == Convert.ToInt32(UserTypeEnum.ExternalSupplier))
                    .FirstOrDefaultAsync();
                if (currentUserRole != null)
                {
                    return _mapper.Map<LoginResponse>(user);
                }
            }
            return null;
        }

        public Users GetUserByJwtId(int id)
        {
            return _dbContext.Users
                         .Where(u => u.Id == id)
                         .FirstOrDefault();
        }

        public async Task<LoginResponse> GetUserByPhoneNumber(string phoneNumber)
        {
            LoginResponse responseModel = new LoginResponse();
            var user = await _dbContext.Users
                                .Where(u => u.mobile.Equals(phoneNumber))
                                .FirstOrDefaultAsync();
            if (user != null)
            {
                var currentUserRole = await _dbContext.UserRoles
                    .Where(r => r.UserId == user.Id && r.UserRoleId == Convert.ToInt32(UserTypeEnum.ExternalSupplier))
                    .FirstOrDefaultAsync();
                if (currentUserRole != null)
                {
                    return _mapper.Map<LoginResponse>(user);
                }
            }
            return null;
        }

        public async Task<int> CreateMobileVerificationRecord(string mobile, string code)
        {

            var mobileObj = await _dbContext.MobileVerifications.FirstOrDefaultAsync(x => x.Mobile == mobile && x.IsAvail == true);
            if (mobileObj != null)
            {
                mobileObj.Code = code;
                mobileObj.IsAvail = false;
                mobileObj.Count += 1;
                mobileObj.CreatedDate = DateTime.UtcNow;
                mobileObj.ModifiedDate = DateTime.UtcNow;
                _dbContext.Entry(mobileObj).State = EntityState.Modified;
                return await _dbContext.SaveChangesAsync();
            }
            MobileVerifications mobileVerifications = new MobileVerifications()
            {
                Mobile = mobile,
                Code = code,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow,
                Count = 0,
                IsAvail = false,
            };
            _dbContext.Entry(mobileVerifications).State = EntityState.Added;
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<LoginResponse> VerifyMobileVerificationRecord(string mobile, string code)
        {

            LoginResponse responseModel = new LoginResponse();
            var mobileObj = await _dbContext.MobileVerifications.FirstOrDefaultAsync(x => x.Mobile == mobile && x.Code == code && x.IsAvail == false);
            if (mobileObj != null)
            {
                mobileObj.IsAvail = true;
                mobileObj.Count += 1;
                mobileObj.ModifiedDate = DateTime.UtcNow;
                _dbContext.Entry(mobileObj).State = EntityState.Modified;
                var result = await _dbContext.SaveChangesAsync();
                if (result > 0)
                {
                    var user = await _dbContext.Users
                                .Where(u => u.mobile.Equals(mobile))
                                .FirstOrDefaultAsync();
                    return _mapper.Map<LoginResponse>(user); 
                }
            }
            return null;
        }
        public async Task<bool> AddFcmToken(string fcmToken)
        {
            var userFcmTokens = _dbContext.UserFcmTokens.FirstOrDefault(u => u.UserId == LoggedInUserId);
            if (userFcmTokens != null)
            {
                userFcmTokens.FcmToken = fcmToken;
                userFcmTokens.ModifiedDate = DateTime.Now;
            }
            else
            {
                userFcmTokens = new UserFcmTokens()
                {
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    UserId = LoggedInUserId,
                    FcmToken = fcmToken
                };
                _dbContext.Entry(userFcmTokens).State = EntityState.Added;
            }
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<LoginResponse> CheckUserWithRole(string mobile)
        {

            LoginResponse responseModel = new LoginResponse();
            var mobileObj = await _dbContext.Users.FirstOrDefaultAsync(x => x.mobile == mobile);
            if (mobileObj != null)
            {
                var role = await _dbContext.UserRoles.FirstOrDefaultAsync(x => x.UserId == mobileObj.Id);
                responseModel.Id = mobileObj.Id;
                responseModel.Mobile = mobileObj.mobile;
                responseModel.Password = mobileObj.Password;
                responseModel.Name = mobileObj.Name;
                responseModel.Role = role.UserRoleId;
                return responseModel;
            }
            return null;
        }

        #endregion
    }
}
