using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NanhiDuniya.Contracts;
using NanhiDuniya.Services.AuthAPI.Constants;
using NanhiDuniya.Services.AuthAPI.Data;
using NanhiDuniya.Services.AuthAPI.Data.IRepositories;
using NanhiDuniya.Services.AuthAPI.Data.Repositories;
using NanhiDuniya.Services.AuthAPI.Middleware;
using NanhiDuniya.Services.AuthAPI.Models;
using NanhiDuniya.Services.AuthAPI.Models.Dto;
using NanhiDuniya.Services.AuthAPI.Service.IService;
using NanhiDuniya.Services.AuthAPI.Utilities;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace NanhiDuniya.Services.AuthAPI.Service
{
    public class AuthService : IAuthService
    {
         private readonly UserManager<ApplicationUser> _userManager;
         private readonly NanhiDuniyaDbContext _db;
         private readonly RoleManager<IdentityRole> _roleManager;
         private readonly ITokenService _tokenService;
         private readonly IUserService _userService;
         private readonly IWebHostEnvironment _env;
         private readonly IPublishEndpoint publishEndpoint;
         private readonly ILogger<AuthService> _logger;
         private readonly JWTService _jwtService;
         private readonly IHttpContextAccessor _httpContextAccessor;
         private readonly IAuthRepository _authRepository;
        public AuthService(NanhiDuniyaDbContext db,
        UserManager<ApplicationUser> userManager, 
        IHttpContextAccessor httpContextAccessor, 
        RoleManager<IdentityRole> roleManager, 
        ITokenService tokenService,
        IUserService userService, 
        IOptions<JWTService> options,
        IAuthRepository authRepository,
        IWebHostEnvironment env, 
        IPublishEndpoint publishEndpoint, 
        ILogger<AuthService> logger)
        {
            _db = db;
            this.publishEndpoint = publishEndpoint;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _tokenService = tokenService;
            _authRepository = authRepository;
            _roleManager = roleManager;
            _jwtService = options.Value;
            _userService = userService;
            _env = env;
            this._logger = logger;
        }

        #region Authentication
        public async Task<ApiResponse<object>> Register(RegistrationRequestDto model)
        {
            // Initialize a ResultResponse object to store the registration result.
            // Check if a user with the same email already exists.
            var userExists = await _userManager.FindByEmailAsync(model.Email);

            // If a user with the same email exists, return a response indicating user already exists.
            if (userExists != null)
            {
                return ApiResponseHelper.CreateErrorResponse<object>("User already exists!", StatusCodes.Status409Conflict);
            }
            var user = BuildUserFromRegistrationModel(model);
            var result = await CreateUserAsync(user, model.Password);

            //Add User Roles
            if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            //if (!await _roleManager.RoleExistsAsync(UserRoles.Student))
            //    await _roleManager.CreateAsync(new IdentityRole(UserRoles.Student));

            //if (!await _roleManager.RoleExistsAsync(UserRoles.Teacher))
            //    await _roleManager.CreateAsync(new IdentityRole(UserRoles.Teacher));

            //if (!await _roleManager.RoleExistsAsync(UserRoles.Parent))
            //    await _roleManager.CreateAsync(new IdentityRole(UserRoles.Parent));

            if (await _roleManager.RoleExistsAsync(UserRoles.Admin) && model.Role == UserRoles.Admin)
            {
                await _userManager.AddToRoleAsync(user, UserRoles.Admin);
            }


            //if (await _roleManager.RoleExistsAsync(UserRoles.Student) && model.Role == UserRoles.Student)
            //{
            //    await _userManager.AddToRoleAsync(user, UserRoles.Student);
            //}

            //if (await _roleManager.RoleExistsAsync(UserRoles.Teacher) && model.Role == UserRoles.Teacher)
            //{
            //    await _userManager.AddToRoleAsync(user, UserRoles.Teacher);
            //}

            //if (await _roleManager.RoleExistsAsync(UserRoles.Parent) && model.Role == UserRoles.Parent)
            //{
            //    await _userManager.AddToRoleAsync(user, UserRoles.Parent);
            //}
            var adminRecord = new Admin
            {
                UserId = user.Id,
                Birthday = model.Birthday,
                Gender = model.Gender,
                Address = model.Address,
                BloodGroup = model.BloodGroup,
            };
           await  _authRepository.InsertAdminRecordAsync(adminRecord);

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
        public async Task<ApiResponse<UserProfile>> Login(LoginRequestDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.UserName);

            if (user == null)
            {
                _logger.LogWarning($"User with email {loginDto.UserName} was not found");
                return ApiResponseHelper.CreateErrorResponse<UserProfile>("User doesn't exist! Please register first", StatusCodes.Status404NotFound);
            }

            bool isValidUser = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!isValidUser)
            {
                return ApiResponseHelper.CreateErrorResponse<UserProfile>("Password is not correct.Please check your password.", StatusCodes.Status401Unauthorized);
            }
            if (!user.EmailConfirmed)
            {
                var userRole = await _userManager.GetRolesAsync(user);
                var userProfile = MapToUserProfile(user, userRole);
                return ApiResponseHelper.CreateEmailNotConfirmedResponse("Email not confirmed. Please confirm your email to proceed.", userProfile);
            }

            var roles = await _userManager.GetRolesAsync(user);
            var token = await _tokenService.GenerateAccessToken(user.Email, user.Id);
            var refreshToken = await _tokenService.GenerateRefreshToken();
            _httpContextAccessor.HttpContext.Response.Cookies.Append("X-Access-Token", token, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.None, Secure = true, Expires = DateTime.Now.AddMinutes(Convert.ToInt32(_jwtService.AccessTokenExpiry)) });
            _httpContextAccessor.HttpContext.Response.Cookies.Append("X-Username", user.UserName, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.None, Secure = true, Expires = DateTime.Now.AddHours(Convert.ToInt32(_jwtService.RefreshTokenExpiry)) });
            _httpContextAccessor.HttpContext.Response.Cookies.Append("X-Refresh-Token", refreshToken, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.None, Secure = true, Expires = DateTime.Now.AddHours(Convert.ToInt32(_jwtService.RefreshTokenExpiry)) });

            var loggedInUser = MapToUserProfile(user, roles);

            var userRefreshToken = new UserRefreshToken
            {
                RefreshToken = refreshToken,
                Expires = DateTime.UtcNow.AddDays(_jwtService.RefreshTokenExpiry),
                Created = DateTime.UtcNow,
                UserId = user.Id,
                IsRevoked = false
            };
            await _tokenService.AddRefreshTokenAsync(userRefreshToken);
            return ApiResponseHelper.CreateSuccessResponse(loggedInUser, "Logged in successfully");
        }
        public async Task<ApiResponse<object>> ForgotPassword(string email)
        {
            var token = await _tokenService.GenerateResetPasswordToken(email);
            if (token == null)
            {
                return ApiResponseHelper.CreateErrorResponse<object>("User Not Found", StatusCodes.Status404NotFound);
            }
            var generatedLink = _userService.GenerateResetPasswordLink(email, token);
            List<string> toEmailLoist = null;
            toEmailLoist ??= new List<string>();
            toEmailLoist.Add(email);
            toEmailLoist.Add("saurabhmishra1464@gmail.com");
            var templatePath = Path.Combine(_env.ContentRootPath, "Templates", $"{"ResetPassword"}.html");
            var htmlBody = LoadHtmlTemplate(templatePath, generatedLink, "", email);
            await publishEndpoint.Publish<SendEmailEvent>(new
            {
                ToEmail = toEmailLoist,
                From = "saurabhmishra1464@gmail.com",
                Subject = "Forgot Password",
                HtmlBody = htmlBody
            });

            return ApiResponseHelper.CreateSuccessResponse<object>(null, "Reset password link generated successfully");
        }
        public async Task<ApiResponse<object>> ResetPassword(ResetPasswordDto model)
        {
            var tokenValidationResult = await ValidResetToken(model.Token, model.Email);
            if (!tokenValidationResult)
            {
                return ApiResponseHelper.CreateErrorResponse<object>("The provided token is invalid. Please check and try again.", StatusCodes.Status400BadRequest);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return ApiResponseHelper.CreateErrorResponse<object>("User not found.", StatusCodes.Status404NotFound);
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);

            if (result.Succeeded)
            {
                return ApiResponseHelper.CreateSuccessResponse<object>(null, "Password reset successfully completed.");
            }
            else
            {
                return ApiResponseHelper.CreateErrorResponse<object>("Failed to reset password.", StatusCodes.Status500InternalServerError);

            }
        }
        public async Task<ApiResponse<object>> ConfirmEmail(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return ApiResponseHelper.CreateErrorResponse<object>("User Not Found", StatusCodes.Status404NotFound); // User not found
            }
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                return ApiResponseHelper.CreateErrorResponse<object>("Email confirmation token has expired. Please request a new confirmation email.", StatusCodes.Status410Gone);
            }

            return ApiResponseHelper.CreateSuccessResponse<object>(null, "Email confirmation succesfully completed.");
        }
        public async Task<ApiResponse<object>> SendConfirmationEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return ApiResponseHelper.CreateErrorResponse<object>("User doesn't exist!", StatusCodes.Status404NotFound);
            }
            var token = await _tokenService.GenerateConfirmEmailToken(email);
            var confirmEmailLink = _userService.GenerateVerifyEmailLink(user.Email, token);

            List<string> toEmailLoist = null;
            toEmailLoist ??= new List<string>();
            toEmailLoist.Add(user.Email);
            toEmailLoist.Add("saurabhmishra1464@gmail.com");
            var templatePath = Path.Combine(_env.ContentRootPath, "Templates", $"{"VerifyEmail"}.html");
            var htmlBody = LoadHtmlTemplate(templatePath, confirmEmailLink, user.FirstName, user.Email);
            await publishEndpoint.Publish<SendEmailEvent>(new
            {
                ToEmail = toEmailLoist,
                From = "saurabhmishra1464@gmail.com",
                Subject = "Confirm Email",
                HtmlBody = htmlBody
            });

            return ApiResponseHelper.CreateSuccessResponse<object>(null, "Email sent Succesfully");
        }

        public async Task<ApiResponse<IEnumerable<AdminDto>>> GetAdmins()
        {
            var admins = await _authRepository.GetAdmins();
            return ApiResponseHelper.CreateSuccessResponse(admins, "UserProfile Fetched Successfully");
        }
        public async Task<ApiResponse<UserProfile>> GetUser()
        {
            var accessToken = _httpContextAccessor.HttpContext.Request.Cookies["X-Access-Token"];
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(accessToken);
            var userIdClaim = jwtSecurityToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier); // "sub" is a common claim type for user ID
            if (userIdClaim == null)
            {
                return ApiResponseHelper.CreateErrorResponse<UserProfile>("UserIdClaim is not found in token", StatusCodes.Status404NotFound);
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
            return ApiResponseHelper.CreateSuccessResponse<UserProfile>(user, "UserProfile Fetched Succesfully");
        }
        public async Task<ApiResponse<UserProfile>> PutUserAsync(UserInfoDto userInfoDto)
        {
            var user = await _userManager.FindByIdAsync(userInfoDto.Id);

            if (user == null)
            {
                return ApiResponseHelper.CreateErrorResponse<UserProfile>("User not found", StatusCodes.Status404NotFound);
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
            if (!result.Succeeded) { return ApiResponseHelper.CreateErrorResponse<UserProfile>("Failed to update user profile. Please try again or contact support if the problem persists.", StatusCodes.Status400BadRequest); }

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
            return ApiResponseHelper.CreateSuccessResponse<UserProfile>(userProfile, "User Updated Successfully");
        }
        



        #endregion


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

        public string LoadHtmlTemplate(string templatePath, string resetLink, string firstName, string to)
        {
            ValidateFileExists(templatePath);
            string htmlContent = File.ReadAllText(templatePath);
            htmlContent = htmlContent.Replace("{{SchoolName}}", "Nanhi Duniya");
            htmlContent = htmlContent.Replace("{{FirstName}}", firstName);
            htmlContent = htmlContent.Replace("{{resetLink}}", resetLink);
            htmlContent = htmlContent.Replace("{{Email}}", to);
            return htmlContent;
        }

        public void ValidateFileExists(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"File not found: {filePath}", filePath);
        }

        private UserProfile MapToUserProfile(ApplicationUser user, IList<string> roles)
        {
            return new UserProfile
            {
                Id = user.Id,
                FullName = user.FirstName + " " + user.LastName,
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                Bio = user.Bio,
                ProfilePictureUrl = user.ProfilePictureUrl,
                Roles = roles.ToList(),
                IEmailConfirmed = user.EmailConfirmed,
            };
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




        #endregion
    }
}
