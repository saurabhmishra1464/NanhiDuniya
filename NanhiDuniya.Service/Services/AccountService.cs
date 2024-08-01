using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NanhiDuniya.Core.Constants;
using NanhiDuniya.Core.Interfaces;
using NanhiDuniya.Core.Models;
using NanhiDuniya.Core.Resources.AccountDtos;
using NanhiDuniya.Data.Entities;
using PhoneNumbers;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
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
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private readonly ILogger<AccountService> _logger;
        public AccountService(NanhiDuniyaDbContext context,
            UserManager<ApplicationUser> userManager,
            IMapper mapper,
            RoleManager<IdentityRole> roleManager,
            IEmailClientService emailClient,
            IUserService userService,
            ILogger<AccountService> logger,
            ITokenService tokenService
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _emailClient = emailClient;
            _mapper = mapper;
            _userService = userService;
            _tokenService = tokenService;
            this._logger = logger;
        }
        #endregion

        #region Authentication Token/Login
        public async Task<LoginResponse> Login(LoginModel loginDto)
        {
           var user = await _userManager.FindByEmailAsync(loginDto.Email);
            bool isValidUser = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if (user == null || isValidUser == false)
            {
                _logger.LogWarning($"User with email {loginDto.Email} was not found");
                return null;
            }

            var token = await _tokenService.GenerateAccessToken(user.Id);
            var refreshToken = await _tokenService.GenerateRefreshToken();

            var loginResponse= new LoginResponse
            {
                Token = token,
                UserId = user.Id,
                RefreshToken = refreshToken,
            };
            var newRefreshToken = _mapper.Map<UserRefreshToken>(loginResponse);
            await _tokenService.AddRefreshTokenAsync(newRefreshToken);
            return loginResponse;
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
                return new ResultResponse { Message = "User already exists!" };
            }

            // Build a user entity from the registration model.
            var user = BuildUserFromRegistrationModel(model);

            // Attempt to create the user asynchronously.
            var result = await CreateUserAsync(user, model.Password);
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
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                // Customize the email template and send reset link

                var resetLink = _userService.GeneratePasswordResetLink(new UserDto { Email = user.Email }, token);
                _ = _emailClient.SendEmailAsync("Registration Successful", model.FirstName, resetLink, null, null, "RegistrationSuccesful", user.Email);


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
        public async Task<ResultResponse> ResetPassword(ResetPasswordDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return new ResultResponse { Message = "User not found." };
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);

            if (result.Succeeded)
            {
                return new ResultResponse { IsSuccess = true, Message = "Password reset successfully." };
            }
            else
            {
                return new ResultResponse { Message = "Failed to reset password." };
            }
        }
        public async Task<ResultResponse> ValidateResetToken(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return new ResultResponse { Message = "User Not Found." }; // User not found
            }

            var result = await _userManager.VerifyUserTokenAsync(user,
                TokenOptions.DefaultProvider, UserManager<ApplicationUser>.ResetPasswordTokenPurpose, token);
            if (!result)
            {
                return new ResultResponse { IsSuccess = true, Message = "Token Expired" };
            }

            return new ResultResponse { IsSuccess = true };
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
