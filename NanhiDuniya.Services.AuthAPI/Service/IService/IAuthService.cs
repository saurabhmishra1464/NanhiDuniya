using NanhiDuniya.Services.AuthAPI.Models.Dto;

namespace NanhiDuniya.Services.AuthAPI.Service.IService
{
    public interface IAuthService
    {
        Task<ApiResponse<object>> Register(RegistrationRequestDto model);
        Task<ApiResponse<UserProfile>> Login(LoginRequestDto model);
        Task<ApiResponse<object>> ForgotPassword(string email);
        Task<ApiResponse<object>> ResetPassword(ResetPasswordDto resetPasswordDto);
        Task<ApiResponse<object>> ConfirmEmail(string token, string email);
        Task<ApiResponse<object>> SendConfirmationEmail(string email);
        Task<ApiResponse<UserProfile>> GetUser();
        Task<ApiResponse<UserProfile>> PutUserAsync(UserInfoDto userInfoDto);
    }
}
