using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using NanhiDuniya.Core.Constants;
using NanhiDuniya.Core.Interfaces;
using NanhiDuniya.Core.Models;
using NanhiDuniya.Data.Entities;
using PhoneNumbers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NanhiDuniya.Service.Services
{
    public class AccountService : IAccountService
    {
        #region Global declarations 
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailClientService _emailClient;
        private readonly IMapper _mapper;
        public AccountService(
            UserManager<ApplicationUser> userManager,
            IMapper mapper,
            RoleManager<IdentityRole> roleManager,
            IEmailClientService emailClient


            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _emailClient = emailClient;
            _mapper = mapper;
        }
        #endregion

        #region Authentication Token/Login
        public async Task<LoginResponse> Login(LoginModel model)
        {

            return new LoginResponse();
        }
        #endregion

        #region User Registration methods

        public async Task<ResultResponse> Register(RegisterModel model)
        {
            // Initialize a ResultResponse object to store the registration result.
            var response = new ResultResponse();
            // Check if a user with the same email already exists.
            var userExists = await FindUserByEmail(model.Email);

            // If a user with the same email exists, return a response indicating user already exists.
            if (userExists != null)
            {
                return UserAlreadyExistsResponse();
            }

            // Build a user entity from the registration model.
            var user = BuildUserFromRegistrationModel(model);

            // Attempt to create the user asynchronously.
            var result = await _userManager.CreateAsync(user);
            // If the user creation is successful, update the response accordingly.
            if (result.Succeeded)
            {
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
                _ = _emailClient.SendEmailAsync("Registration Successful", null, null, "VerifyEmail", user.Email);


                response.IsSuccess = true;
                response.Message = "User registered successfully!";
            }
            else
            {
                // If user creation fails, build an error message from the result errors.
                response.Message = BuildErrorMessage(result.Errors);
            }
            // Return the final response.
            return response;
        }

        #endregion

        #region Helper methods
        private async Task<IdentityResult> CreateUserAsync(ApplicationUser user, string? password)
        {
            return await _userManager.CreateAsync(user, password!);
        }
        private static ApplicationUser BuildUserFromRegistrationModel(RegisterModel model)
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
        private static ResultResponse UserAlreadyExistsResponse()
        {
            return new ResultResponse { Message = "User already exists!" };
        }
        private async Task<ApplicationUser?> FindUserByEmail(string? email)
        {
            // Logic to find a user by email using the user manager
            return await _userManager.FindByEmailAsync(email!);
        }

        private string BuildErrorMessage(IEnumerable<IdentityError> errors)
        {
            return string.Join(", ", errors.Select(error => error.Description));
        }
        #endregion
    }
}
