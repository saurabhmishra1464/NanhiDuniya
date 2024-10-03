namespace NanhiDuniya.Services.AuthAPI.Service.IService
{
    public interface IPasswordService
    {
        string HashPassword(string password);
        bool VerifyPassword(string hashedPassword, string providedPassword);
    }

}
