using NanhiDuniya.Services.AuthAPI.Models;

namespace NanhiDuniya.Services.AuthAPI.Data.IRepositories
{
    public interface IAuthRepository
    {
        public Task InsertAdminRecordAsync(Admin admin);
    }
}
