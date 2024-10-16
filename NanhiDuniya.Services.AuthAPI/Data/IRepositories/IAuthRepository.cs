using NanhiDuniya.Services.AuthAPI.Models;
using NanhiDuniya.Services.AuthAPI.Models.Dto;

namespace NanhiDuniya.Services.AuthAPI.Data.IRepositories
{
    public interface IAuthRepository
    {
        Task InsertAdminRecordAsync(Admin admin);
        Task<IEnumerable<AdminDto>> GetAdmins();
    }
}
