using Microsoft.AspNetCore.Identity;
using NanhiDuniya.Services.AuthAPI.Data;
using NanhiDuniya.Services.AuthAPI.Middleware;
using NanhiDuniya.Services.AuthAPI.Models;
using NanhiDuniya.Services.AuthAPI.Models.Dto;
using NanhiDuniya.Services.AuthAPI.Service.IService;
using System;

namespace NanhiDuniya.Services.AuthAPI.Service
{
    public class AuthService : IAuthService
    {
         private readonly UserManager<ApplicationUser> _userManager;
         private readonly NanhiDuniyaDbContext _db;
         private readonly IJwtTokenGenerator _jwtTokenGenerator;
         private readonly RoleManager<IdentityRole> _roleManager;
        public AuthService(NanhiDuniyaDbContext db, IJwtTokenGenerator jwtTokenGenerator,
    UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _jwtTokenGenerator = jwtTokenGenerator;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<ApiResponse<object>> Register(RegistrationRequestDto model)
        {
            // Initialize a ResultResponse object to store the registration result.
            // Check if a user with the same email already exists.
            var userExists = await _userManager.FindByEmailAsync(model.Email);

            // If a user with the same email exists, return a response indicating user already exists.
            if (userExists != null)
            {
                throw new UserAlreadyExistsException("User already exists!");
            }
            var user = BuildUserFromRegistrationModel(model);
            var result = await CreateUserAsync(user, model.Password);

            //Add User Roles
            if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));

            if (!await _roleManager.RoleExistsAsync(UserRoles.Student))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Student));

            if (!await _roleManager.RoleExistsAsync(UserRoles.Teacher))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Teacher));

            if (!await _roleManager.RoleExistsAsync(UserRoles.Parent))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Parent));

            if (await _roleManager.RoleExistsAsync(UserRoles.Admin) && model.Role == UserRoles.Admin)
            {
                await _userManager.AddToRoleAsync(user, UserRoles.Admin);
            }

            if (await _roleManager.RoleExistsAsync(UserRoles.Student) && model.Role == UserRoles.Student)
            {
                await _userManager.AddToRoleAsync(user, UserRoles.Student);
            }

            if (await _roleManager.RoleExistsAsync(UserRoles.Teacher) && model.Role == UserRoles.Teacher)
            {
                await _userManager.AddToRoleAsync(user, UserRoles.Teacher);
            }

            if (await _roleManager.RoleExistsAsync(UserRoles.Parent) && model.Role == UserRoles.Parent)
            {
                await _userManager.AddToRoleAsync(user, UserRoles.Parent);
            }
            var token = await _tokenService.GenerateConfirmEmailToken(user.Email);

            // Customize the email template and send verify email link

            var verifyEmailLink = _userService.GenerateVerifyEmailLink(user.Email, token);

            List<string> toEmailLoist = null;
            toEmailLoist ??= new List<string>();
            toEmailLoist.Add(user.Email);
            toEmailLoist.Add("saurabhmishra1464@gmail.com");
            var templatePath = Path.Combine(_env.ContentRootPath, "Templates", $"{"VerifyEmail"}.html");
            var htmlBody = LoadHtmlTemplate(templatePath, verifyEmailLink, user.FirstName, user.Email);

            await publishEndpoint.Publish<SendEmailEvent>(new
            {
                ToEmail = toEmailLoist,
                From = "saurabhmishra1464@gmail.com",
                Subject = "Confirm Email",
                HtmlBody = htmlBody
            });
            _logger.LogInformation("User registered successfully: {Email}", model.Email);
            return ApiResponseHelper.CreateSuccessResponse<object>(null, "User Registered Succesfully");
        }




        #region Helper methods

        private static ApplicationUser BuildUserFromRegistrationModel(RegistrationRequestDto model)
        {
            return new ApplicationUser
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PasswordHash = model.Password,
                PhoneNumber = model.PhoneNumber,
            };
        }

        private async Task<IdentityResult> CreateUserAsync(ApplicationUser user, string? password)
        {
            return await _userManager.CreateAsync(user, password!);
        }

        #endregion
    }
}
