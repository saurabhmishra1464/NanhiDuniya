using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using NanhiDuniya.Core.Constants;
using NanhiDuniya.Core.Interfaces;
using NanhiDuniya.Core.Models;
using NanhiDuniya.Core.Models.Exceptions;
using NanhiDuniya.Core.Resources.AccountDtos;
using NanhiDuniya.Data.Entities;
using NanhiDuniya.Data.Repositories;
using NanhiDuniya.Service.Services;
using NanhiDuniya.UserManagement.Api.Extentions;
using NanhiDuniya.UserManagement.Api.Middleware;
using Newtonsoft.Json.Linq;
using System.Runtime.Intrinsics.X86;
using System.Security.Claims;

namespace NanhiDuniya.UserManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        #region Global declarations
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;
        private readonly ILogger<AccountController> _logger;
        private readonly IPasswordService _passwordService;
        private readonly ITokenService _tokenService;
        private readonly IImageService _imageService;
        public AccountController(IAccountService accountService, IImageService imageService, IPasswordService passwordService, ITokenService tokenService, IMapper mapper, ILogger<AccountController> logger)
        {
            _accountService = accountService;
            _passwordService = passwordService;
            _tokenService = tokenService;
            _imageService = imageService;
            _mapper = mapper;
            _logger = logger;
        }
        #endregion

        #region User Registration Endpoint

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDto model)
        {

            var result = await _accountService.Register(_mapper.Map<RegisterModel>(model));

            if (result.Success)
            {
                _logger.LogInformation("User registered successfully: {Email}", model.Email);
                return Ok(new ApiResponse(StatusCodes.Status201Created, result.Message));
            }

            _logger.LogError("Registration failed for user {Email}: {ErrorMessage}", model.Email, result.Message);
            throw new RegsitrationFailed("Registration Failed for user");

        }
        #endregion

        #region Admin Registration Endpoint

        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost("Register-Admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegistrationRequestDto model)
        {

            var result = await _accountService.Register(_mapper.Map<RegisterModel>(model));

            if (result.Success)
            {
                _logger.LogInformation("Admin registered successfully: {Email}", model.Email);
                return Ok(new ApiResponse(StatusCodes.Status201Created, result.Message));
            }

            _logger.LogError("Registration failed for admin {Email}: {ErrorMessage}", model.Email, result.Message);
            throw new RegsitrationFailed("Registration Failed for admin");
        }
        #endregion

        #region Authentication Token and handshake endpoints 
        //[HttpPost("Login")]
        //public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
        //{
        //    var result = await _accountService.Login(_mapper.Map<LoginModel>(model));

        //    if (result == null)
        //    {
        //        _logger.LogWarning("Login attempt failed for user: {Username}", model.Email);
        //        throw new UnauthorizedAccessException("Invalid login credentials.");
        //    }
        //    _logger.LogInformation("User {Username} logged in successfully.", model.Email);
        //    return Ok(result);
        //}

        [HttpPost("login")]
        public async Task<IActionResult> LoginApi([FromBody] LoginModel model)
        {
            var result = await _accountService.Login(_mapper.Map<LoginModel>(model));
            if (result == null)
            {
                _logger.LogWarning("Login attempt failed for user: {Username}", model.UserName);
                throw new UnauthorizedAccessException("Invalid login credentials.");
            }
            return Ok(result);
        }



        //[HttpPost("RefreshToken")]
        //public async Task<IActionResult> RefreshToken()
        //{
        //    var refreshToken = Request.Cookies["refreshToken"];
        //    var response = await _tokenService.VerifyRefreshToken(refreshToken);
        //    if (response == null)
        //    {
        //        throw new UnauthorizedAccessException();
        //    }

        //    return Ok(response);
        //}

        [HttpGet("refresh")]
        public async Task<IActionResult> Refresh()
        {
            if (!(Request.Cookies.TryGetValue("X-Username", out var userName) && Request.Cookies.TryGetValue("X-Refresh-Token", out var refreshToken)))
                throw new ArgumentNullException();

            var result = await _tokenService.VerifyRefreshToken(refreshToken, userName);

            return Ok(result);
        }

        [HttpGet("check-auth")]
        [Authorize]
        public IActionResult CheckAuth()
        {
            var userClaims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();
            return Ok(new {Success=true, message = "User is authenticated", claims = userClaims });
        }



        [HttpPost("RevokeRefreshToken")]
        [Authorize]
        public async Task<IActionResult> RevokeRefreshToken(RevokeRefreshTokenRequest revokeRefreshTokenRequest)
        {
            var result = await _tokenService.RevokeRefreshToken(revokeRefreshTokenRequest.UserId);

            Response.Cookies.Delete("X-Access-Token", new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });
            Response.Cookies.Delete("X-Username", new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });
            Response.Cookies.Delete("X-Refresh-Token", new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });

            return Ok(result);
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto model)
        {
            var tokenValidationResult = await _accountService.ValidateResetToken(model.Token, model.Email);
            if (!tokenValidationResult.Success)
            {
                throw new ArgumentException("The provided token is invalid. Please check and try again.");
            }
            var result = await _accountService.ResetPassword(model);

            if (result.Success)
            {
                _logger.LogInformation("Password reset successfully for user: {Email}", model.Email);
                return Ok(new ApiResponse(StatusCodes.Status200OK, result.Message));
            }

            _logger.LogError("Password reset failed for user {Email}: {ErrorMessage}", model.Email, result.Message);
            throw new InvalidOperationException("Unable to complete the password reset due to a conflict.");
        }

        [HttpGet("Verify-Email")]
        public async Task<IActionResult> VerifyEmail(string token, string email)
        {
            var tokenValidationResult = await _accountService.ConfirmEmail(token, email);
            if (!tokenValidationResult.Success)
            {
                throw new ArgumentException("The provided token is invalid. Please check and try again.");
            }
            //var result = await _accountService.ResetPassword(model);
            if (tokenValidationResult.Success)
            {
                _logger.LogInformation("Email Confirmed for user: {Email}", email);
                return Ok(new ApiResponse(StatusCodes.Status200OK, tokenValidationResult.Message));
            }

            _logger.LogError("Confirm email for user {Email}: {ErrorMessage}", email, tokenValidationResult.Message);
            throw new InvalidOperationException("Unable to complete the verify  email due to a conflict.");
        }


        #endregion

        #region GetUserProfile
        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetUser()
        {
            var accessToken = Request.Cookies["X-Access-Token"];
            var user = await _accountService.GetUser(accessToken);

            return Ok(user);

        }

        #endregion

        #region Update User

        [HttpPut("UpdateUser")]
        [Authorize]
        public async Task<IActionResult> UpdateUser(UserInfoDto userInfo)
        {
            try
            {
                var result = await _accountService.PutUserAsync(userInfo);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw new FailedToUpdate("Failed to Update user with id {userInfo.Id}", ex);
            }
        }

        [HttpPost("UploadProfilePicture")]
        [Authorize]
        public async Task<IActionResult> UploadProfilePicture(UploadProfilePictureDto upload)
        {
            var result = await _imageService.SaveImageAsync(upload);
            if (result.Success)
            {
                _logger.LogInformation("Image Uploaded Successfully");
                return Ok(result);
            }
            _logger.LogError("Image Upload failed for user {Id}: {ErrorMessage}"/*,upload.Id*/, result.Message);
            throw new FailedToUpdate("Image Upload failed.");
        }

        #endregion
    }



}
