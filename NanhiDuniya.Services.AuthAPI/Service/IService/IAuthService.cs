using NanhiDuniya.Services.AuthAPI.Models.Dto;

namespace NanhiDuniya.Services.AuthAPI.Service.IService
{
    public interface IAuthService
    {
        //Task<ApiResponse<UserProfile>> Login(LoginModel model);
        Task<ApiResponse<object>> Register(RegistrationRequestDto model);
    }
}
