using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NanhiDuniya.Core.Constants;
using NanhiDuniya.Core.Interfaces;
using NanhiDuniya.Core.Models;
using NanhiDuniya.Data.Entities;
using PhoneNumbers;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
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
        private readonly JWTService _jwtService;
        public AccountService(
            UserManager<ApplicationUser> userManager,
            IMapper mapper,
            IOptions<JWTService> options,
            RoleManager<IdentityRole> roleManager,
            IEmailClientService emailClient


            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtService = options.Value;
            _emailClient = emailClient;
            _mapper = mapper;
        }
        #endregion

        #region Authentication Token/Login
        public async Task<LoginResponse> Login(LoginModel model)
        {
            // Attempt to find a user by their email address
            var user = await FindUserByEmail(model.Email);
            // Check if a user with the given email exists
            if (user != null)
            {
                // Validate the provided password against the user's stored password hash
                if (await ValidatePassword(user, model.Password))
                {
                    // Generate authentication token and build login response
                    return await BuildLoginResponse(user);
                }
                else
                {
                    // Return a response indicating that the provided password is incorrect
                    return IncorrectPasswordResponse();
                }
            }
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

        private async Task<bool> ValidatePassword(ApplicationUser user, string? password)
        {
            // Logic to validate the provided password against the user's stored password hash
            return await _userManager.CheckPasswordAsync(user, password!);
        }

        private async Task<LoginResponse> BuildLoginResponse(ApplicationUser user)
        {
            // Retrieve user roles and generate authentication token
            var userRoles = await _userManager.GetRolesAsync(user);
            var authClaims = BuildAuthClaims(user, userRoles);
            var token = GetToken(authClaims);

            // Return a successful login response with the generated token and user information
            return new LoginResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                UserID = user.Id,
                ExpirationTime = token.ValidTo,
                IsSuccess = true,
            };
        }

        private static List<Claim> BuildAuthClaims(ApplicationUser user, IList<string> userRoles)
        {
            // Logic to build a list of claims for the authentication token
            var authClaims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()!),
                new("FirstName", user.FirstName ?? string.Empty),
                new("LastName", user.LastName ?? string.Empty),
                new("Email", user.Email!),
                new("UserID", user.Id.ToString()!),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            List<string> roleList = [];
            // Add each user role as a claim with the type "Role"
            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                roleList.Add(userRole);
            }

            authClaims.Add(new Claim("UserRoles", Newtonsoft.Json.JsonConvert.SerializeObject(roleList)));

            return authClaims;
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtService.Secret!));

            var token = new JwtSecurityToken(
                issuer: _jwtService.ValidIssuer,
                audience: _jwtService.ValidAudience,
                expires: DateTime.UtcNow.AddDays(1),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }

        private static LoginResponse IncorrectPasswordResponse()
        {
            // Return a response indicating that the provided password is incorrect
            return new LoginResponse { Message = "Incorrect password!" };
        }
        #endregion
    }
}
