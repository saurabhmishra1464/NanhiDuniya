using AutoMapper;
using Azure.Core;
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
using NanhiDuniya.Data.Repositories;
using NanhiDuniya.Service.Services;
using NanhiDuniya.UserManagement.Api.Extentions;
using Newtonsoft.Json.Linq;
using System.Runtime.Intrinsics.X86;

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
        public AccountController(IAccountService accountService, IPasswordService passwordService, ITokenService tokenService, IMapper mapper, ILogger<AccountController> logger)
        {
            _accountService = accountService;
            _passwordService = passwordService;
            _tokenService = tokenService;
            _mapper = mapper;
            _logger = logger;
        }
        #endregion

        #region User Registration Endpoint

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Validation failed."));
            }

            var result = await _accountService.Register(_mapper.Map<RegisterModel>(model));

            if (result.IsSuccess)
            {
                _logger.LogInformation("User registered successfully: {Email}", model.Email);
                return Ok(new ApiResponse(StatusCodes.Status201Created, result.Message));
            }

            _logger.LogError("Registration failed for user {Email}: {ErrorMessage}", model.Email, result.Message);
            return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, result.Message));

        }
        #endregion

        #region Admin Registration Endpoint

        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost("Register-Admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegistrationRequestDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Validation failed."));
            }

            var result = await _accountService.Register(_mapper.Map<RegisterModel>(model));

            if (result.IsSuccess)
            {
                _logger.LogInformation("Admin registered successfully: {Email}", model.Email);
                return Ok(new ApiResponse(StatusCodes.Status201Created, result.Message));
            }

            _logger.LogError("Registration failed for user {Email}: {ErrorMessage}", model.Email, result.Message);
            return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, result.Message));
        }
        #endregion

        #region Authentication Token and handshake endpoints 
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
        {
            var result = await _accountService.Login(_mapper.Map<LoginModel>(model));
           
            if (result == null)
            {
                _logger.LogWarning("Login attempt failed for user: {Username}", model.Email);
                return Unauthorized();
            }
            _logger.LogInformation("User {Username} logged in successfully.", model.Email);
            return Ok(result);
        }

        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto request)
        {
            var authResponse = await _tokenService.VerifyRefreshToken(request);
            if (authResponse==null)
            {
                throw new UnauthorizedAccessException();
            }

            return Ok(new { authResponse.Token,authResponse.RefreshToken });
        }
        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost("RevokeRefreshToken")]
        public async Task<IActionResult> RevokeRefreshToken([FromBody] RevokeRefreshTokenRequest revokeRefreshTokenRequest)
        {
            await _tokenService.RevokeRefreshToken(revokeRefreshTokenRequest.UserId);
           
            return Ok();
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Validation failed."));
            }
            var tokenValidationResult = await _accountService.ValidateResetToken(model.Token, model.Email);
            if (!tokenValidationResult.IsSuccess)
            {
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, tokenValidationResult.Message));
            }
            var result = await _accountService.ResetPassword(model);

            if (result.IsSuccess)
            {
                _logger.LogInformation("Password reset successfully for user: {Email}", model.Email);
                return Ok(new ApiResponse(StatusCodes.Status200OK, result.Message));
            }

            _logger.LogError("Password reset failed for user {Email}: {ErrorMessage}", model.Email, result.Message);
            return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, result.Message));
        }

        //best approach for resetPassword

//        Generate Token: Use GeneratePasswordResetTokenAsync to generate the token.
//Create a Token Entity: Define a PasswordResetToken entity with properties like UserId, Token, CreatedOn, ExpiresOn, and Used.

//Store Token: Save the generated token and related information in the database.

//Token Verification: When a user attempts to reset their password, retrieve the token from the database, verify its validity, and use ResetPasswordAsync to reset the password.

//Token Invalidation: Mark the token as used or deleted after successful password reset.


        //public async Task<bool> ResetPasswordAsync(string email, string token, string newPassword)
        //{
        //    var user = await _userManager.FindByEmailAsync(email);
        //    if (user == null)
        //    {
        //        return false;
        //        // Or handle error appropriately
        //    }

        //    var passwordResetToken = await _context.PasswordResetTokens
        //        .FirstOrDefaultAsync(t => t.UserId == user.Id && t.Token == token && !t.Used && t.ExpiresOn > DateTime.UtcNow);

        //    if (passwordResetToken == null)
        //    {
        //        return false; // Or handle error appropriately
        //    }

        //    var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
        //    if (result.Succeeded)
        //    {
        //        passwordResetToken.Used = true;
        //        await _context.SaveChangesAsync();
        //    }

        //    return result.Succeeded;
        //}

        #endregion
    }



}
