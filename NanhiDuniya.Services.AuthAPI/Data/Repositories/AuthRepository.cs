using NanhiDuniya.Services.AuthAPI.Data.IRepositories;
using NanhiDuniya.Services.AuthAPI.Models;

namespace NanhiDuniya.Services.AuthAPI.Data.Repositories
{
    public class AuthRepository: IAuthRepository
    {
        private readonly NanhiDuniyaDbContext _dbContext;
        public AuthRepository(NanhiDuniyaDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task InsertAdminRecordAsync(Admin admin)
        {
           await  _dbContext.Admins.AddAsync(admin);
            await _dbContext.SaveChangesAsync();
        }
    }
}
