namespace NanhiDuniya.Services.AuthAPI.Service.IService
{
    public interface IUserService
    {
        string GenerateVerifyEmailLink(string email, string token);
        string GenerateResetPasswordLink(string email, string token);
    }
}
