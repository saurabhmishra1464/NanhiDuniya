using AutoMapper;
using Azure.Core;
using Azure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using NanhiDuniya.Core.Resources.ResponseDtos;
using NanhiDuniya.Core.Models.Exceptions;
using System.Net;

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
        private readonly IImageService _imageService;
        private readonly ILogger<AccountService> _logger;
        private readonly JWTService _jwtService;
        private readonly IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _storagePath;

        public AccountService(NanhiDuniyaDbContext context,
            UserManager<ApplicationUser> userManager,
            IMapper mapper,
            RoleManager<IdentityRole> roleManager,
            IEmailClientService emailClient,
            IUserService userService,
            IImageService imageService,
            ILogger<AccountService> logger,
            ITokenService tokenService,
            IOptions<JWTService> options,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            IWebHostEnvironment env
            )
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _roleManager = roleManager;
            _emailClient = emailClient;
            _mapper = mapper;
            _userService = userService;
            _imageService = imageService;
            _tokenService = tokenService;
            _jwtService = options.Value;
            _env = env;
            _storagePath = configuration["FileStoragePath"];
            this._logger = logger;
        }
        #endregion

        #region Authentication Token/Login
        public async Task<ApiResponse<UserProfile>> Login(LoginModel loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.UserName);

            if (user == null)
            {
                _logger.LogWarning($"User with email {loginDto.UserName} was not found");
                return new ApiResponse<UserProfile>(false, "User doesn't exist! Please register first", null, StatusCodes.Status404NotFound, null);

            }

            bool isValidUser = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!isValidUser)
            {
                return new ApiResponse<UserProfile>(false, "Incorrect Password", null, StatusCodes.Status401Unauthorized, null);
            }

            var roles = await _userManager.GetRolesAsync(user);
            var token = await _tokenService.GenerateAccessToken(user.Email, user.Id);
            var refreshToken = await _tokenService.GenerateRefreshToken();
            _httpContextAccessor.HttpContext.Response.Cookies.Append("X-Access-Token", token, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict, Expires = DateTime.Now.AddMinutes(Convert.ToInt32(_jwtService.AccessTokenExpiry)) });
            _httpContextAccessor.HttpContext.Response.Cookies.Append("X-Username", user.UserName, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict, Expires = DateTime.Now.AddHours(Convert.ToInt32(_jwtService.RefreshTokenExpiry)) });
            _httpContextAccessor.HttpContext.Response.Cookies.Append("X-Refresh-Token", refreshToken, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict, Expires = DateTime.Now.AddHours(Convert.ToInt32(_jwtService.RefreshTokenExpiry)) });


            var loggedInUser = new UserProfile
            {
                Id = user.Id,
                FullName = user.FirstName + " " + user.LastName,
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                Bio = user.Bio,
                ProfilePictureUrl = user.ProfilePictureUrl,
                Roles = roles.ToList(),
            };

            var userRefreshToken = new UserRefreshToken
            {
                RefreshToken = refreshToken,
                Expires = DateTime.UtcNow.AddDays(_jwtService.RefreshTokenExpiry),
                Created = DateTime.UtcNow,
                UserId = user.Id,
                IsRevoked = false
            };
            await _tokenService.AddRefreshTokenAsync(userRefreshToken);
            return new ApiResponse<UserProfile>(true, "LoggedIn SuccesFully", loggedInUser, StatusCodes.Status200OK, null);
        }


        #endregion

        #region User Registration methods

        public async Task<ApiResponse<object>> Register(RegisterModel model)
        {
            // Initialize a ResultResponse object to store the registration result.
            // Check if a user with the same email already exists.
            var userExists = await FindUserByEmail(model.Email);

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
            _ = _emailClient.SendEmailAsync("Registration Successful", model.FirstName, verifyEmailLink, null, null, "VerifyEmail", user.Email);
            _logger.LogInformation("User registered successfully: {Email}", model.Email);
            return new ApiResponse<object>(true, "User Registered Succesfully", null, StatusCodes.Status200OK, null);
        }

        #endregion

        #region Update User
        public async Task<ApiResponse<UserProfile>> PutUserAsync(UserInfoDto userInfoDto)
        {
            var user = await _userManager.FindByIdAsync(userInfoDto.Id);

            if (user == null)
            {
                return new ApiResponse<UserProfile>(false, "User not found", null, StatusCodes.Status404NotFound, null);
            }

            var names = userInfoDto.FullName?.Split(' ') ?? new string[0];
            if (names.Length > 0)
            {
                user.FirstName = names[0];
                user.LastName = names.Length > 1 ? string.Join(" ", names.Skip(1)) : string.Empty;
            }
            user.PhoneNumber = userInfoDto.PhoneNumber;
            user.Email = userInfoDto.Email;
            user.Bio = userInfoDto.Bio;

            var result = await _userManager.UpdateAsync(user);
            if(!result.Succeeded) { return new ApiResponse<UserProfile>(false, "Failed to update user profile. Please try again or contact support if the problem persists.", null, StatusCodes.Status400BadRequest, null); }

                var userProfile = new UserProfile
                {
                    Id = user.Id,
                    FullName = user.FirstName + user.LastName,
                    UserName = user.UserName,
                    PhoneNumber = user.PhoneNumber,
                    Email = user.Email,
                    Bio = user.Bio,
                    ProfilePictureUrl = user.ProfilePictureUrl,
                };
                return new ApiResponse<UserProfile>(true, "User Updated Successfully", userProfile, StatusCodes.Status200OK, null);
        }

        public async Task<ApiResponse<UserProfile>> GetUser()
        {
            var accessToken = _httpContextAccessor.HttpContext.Request.Cookies["X-Access-Token"];
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(accessToken);
            var userIdClaim = jwtSecurityToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier); // "sub" is a common claim type for user ID
            if (userIdClaim == null)
            {
                return new ApiResponse<UserProfile>(false, "UserIdClaim is not found in token", null, StatusCodes.Status404NotFound, null);
            }
            var userId = userIdClaim.Value;
            var user = await _userManager.Users.Where(u => u.Id == userId).Select(u => new UserProfile
            {
                Id = u.Id,
                FullName = u.FirstName + " " + u.LastName,
                PhoneNumber = u.PhoneNumber,
                Email = u.Email,
                Bio = u.Bio,
                UserName = u.UserName,
                ProfilePictureUrl = u.ProfilePictureUrl,
            }).FirstOrDefaultAsync();
            return new ApiResponse<UserProfile>(true, "UserProfile Fetched Succesfully", user, StatusCodes.Status200OK, null);
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

        #region Token

        public async Task<ApiResponse<object>> ForgotPassword(string email)
        {
            var token = await _tokenService.GenerateResetPasswordToken(email);
            if (token == null)
            {
                return new ApiResponse<object>(false, "User Not Found", null, StatusCodes.Status404NotFound, null);
            }
            var generatedLink = _userService.GenerateResetPasswordLink(email, token);
            _ = _emailClient.SendEmailAsync("Forgot Password", "", generatedLink, null, null, "ResetPassword", email);

            return new ApiResponse<object>(true, "Reset password link generated successfully", null, StatusCodes.Status200OK, null);
        }
        public async Task<ApiResponse<object>> ResetPassword(ResetPasswordDto model)
        {
            var tokenValidationResult = await ValidResetToken(model.Token, model.Email);
            if (!tokenValidationResult)
            {
                return new ApiResponse<object>(false, "The provided token is invalid. Please check and try again.", null, StatusCodes.Status400BadRequest, null);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return new ApiResponse<object>(false, "User not found.", null, StatusCodes.Status404NotFound, null);
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);

            if (result.Succeeded)
            {
                return new ApiResponse<object>(true, "Password reset successfully completed.", null, StatusCodes.Status200OK, null);
            }
            else
            {
                return new ApiResponse<object>(false, "Failed to reset password.", null, StatusCodes.Status500InternalServerError, null);

            }
        }
        public async Task<bool> ValidResetToken(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return false; // User not found
            }

            var result = await _userManager.VerifyUserTokenAsync(user,
                TokenOptions.DefaultProvider, UserManager<ApplicationUser>.ResetPasswordTokenPurpose, token);
            if (!result)
            {
                return false;
            }

            return true;
        }
        public async Task<ApiResponse<object>> ConfirmEmail(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return new ApiResponse<object>(false, "User Not Found", null, StatusCodes.Status404NotFound, null); // User not found
            }
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                return new ApiResponse<object>(false, "Email confirmation token has expired. Please request a new confirmation email.", null, StatusCodes.Status410Gone, null);
            }

            return new ApiResponse<object>(true, "Email confirmation succesfully completed.", null, StatusCodes.Status200OK, null);
        }

        #endregion
    }
}
