using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NanhiDuniya.Core.Constants;
using NanhiDuniya.Core.Interfaces;
using NanhiDuniya.Core.Models;
using NanhiDuniya.Core.Models.Exceptions;
using NanhiDuniya.Core.Resources.AccountDtos;
using NanhiDuniya.Service.Services;
using NanhiDuniya.UserManagement.Api.Extentions;

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

        public AccountController(IAccountService accountService, IPasswordService passwordService, IMapper mapper, ILogger<AccountController> logger)
        {
            _accountService = accountService;
            _passwordService = passwordService;
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

        //[HttpPost("RefreshToken")]
        //public async Task<ActionResult> RefreshToken([FromBody] RefreshTokenDto request)
        //{
        //    var authResponse = await _accountService.VerifyRefreshToken(request);

        //    if (authResponse == null)
        //    {
        //        return Unauthorized();
        //    }

        //    return Ok(authResponse);
        //}

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

        #endregion
    }



}
