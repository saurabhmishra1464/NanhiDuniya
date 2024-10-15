using MassTransit.Initializers.Variables;
using Microsoft.EntityFrameworkCore;
using NanhiDuniya.Services.AuthAPI.Data.IRepositories;
using NanhiDuniya.Services.AuthAPI.Models;
using NanhiDuniya.Services.AuthAPI.Models.Dto;

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

        public async Task<IEnumerable<AdminDto>> GetAdmins()
        {
            var admins = await _dbContext.Admins
                .Include(a => a.User)  // Eagerly load related User entity
                .Select(a => new AdminDto
                {
                    Id = a.Id,
                    FirstName = a.User.FirstName,
                    LastName = a.User.LastName,
                    Email = a.User.Email,
                    PhoneNumber = a.User.PhoneNumber,
                    Address = a.Address,
                    BloodGroup = a.BloodGroup,
                    Status = a.User.LockoutEnabled
                })
                .ToListAsync();

            return admins;
        }
    }
}
