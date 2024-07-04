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

        //#region Authentication Token and handshake endpoints 

        //[HttpPost("Login")]
        //public async Task<IActionResult> Login(LoginResource model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var result = await _accountService.Login(_mapper.Map<LoginModel>(model));

        //    if (result.IsSuccess)
        //    {
        //        return Ok(_mapper.Map<LoginResponseResource>(result));
        //    }

        //    _logger.LogWarning($"Login failed for {model.Email}");
        //    return BadRequest(new LoginResponseResource { IsSuccess = false, Message = result.Message });
        //}

        //#endregion


        #region Authentication Token and handshake endpoints 
        /// <summary>
        /// Customer Login
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request</response>
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginRequestDto model)
        {
                if (ModelState.IsValid)
                {
                    var result = await _accountService.Login(_mapper.Map<LoginModel>(model));
                    if (!result.IsSuccess)
                        return BadRequest(_mapper.Map<LoginResponseResource>(result));
                    return Ok(_mapper.Map<LoginResponseResource>(result));
                }

                return BadRequest(new LoginResponseResource() { IsSuccess = false, Message = StaticData.GenericExceptionMessage });
            
        }
        #endregion
    }



}
