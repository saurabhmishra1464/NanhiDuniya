using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NanhiDuniya.Services.AuthAPI.Constants;
using NanhiDuniya.Services.AuthAPI.Models.Dto;
using NanhiDuniya.Services.AuthAPI.Service.IService;

namespace NanhiDuniya.Services.AuthAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        #region Global declarations
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;
        private readonly ILogger<AccountController> _logger;
        private readonly IPasswordService _passwordService;
        private readonly ITokenService _tokenService;
        private readonly IImageService _imageService;
        private readonly IUserService _userService;
        public AccountController(IAuthService accountService, IUserService userService, IImageService imageService, IPasswordService passwordService, ITokenService tokenService, IMapper mapper, ILogger<AccountController> logger)
        {
            _authService = accountService;
            _passwordService = passwordService;
            _tokenService = tokenService;
            _imageService = imageService;
            _userService = userService;
            _mapper = mapper;
            _logger = logger;
        }
        #endregion

        #region User Registration Endpoint

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDto model)
        {
            var result = await _authService.Register(model);
            return Ok(result);

        }
        #endregion

        #region Admin Registration Endpoint

        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost("Register-Admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegistrationRequestDto model)
        {
            var result = await _authService.Register(model);
            return Ok(result);
        }
        #endregion

        //#region Authentication Token and handshake endpoints 

        //[HttpPost("login")]
        //public async Task<IActionResult> LoginApi([FromBody] LoginRequestDto model)
        //{
        //    var result = await _accountService.Login(_mapper.Map<LoginModel>(model));
        //    return Ok(result);
        //}

        //[HttpPost("forgot-password")]
        //public async Task<IActionResult> ForgotPassword(ForgotPasswordDto forgotPasswordDto)
        //{
        //    var resetPassword = await _accountService.ForgotPassword(forgotPasswordDto.Email);

        //    return Ok(resetPassword);
        //}

        //[HttpGet("refresh")]
        //public async Task<IActionResult> Refresh()
        //{
        //    var result = await _tokenService.VerifyRefreshToken();

        //    return Ok(result);
        //}

        //[HttpGet("check-auth")]
        //public IActionResult CheckAuth()
        //{
        //    var userClaims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();
        //    return Ok(new { Success = true, message = "User is authenticated", claims = userClaims });
        //}



        //[HttpPost("RevokeRefreshToken")]
        //[Authorize]
        //public async Task<IActionResult> RevokeRefreshToken(RevokeRefreshTokenDto revokeRefreshTokenRequest)
        //{
        //    var result = await _tokenService.RevokeRefreshToken(revokeRefreshTokenRequest.UserId);
        //    return Ok(result);
        //}

        //[HttpPost("ResetPassword")]
        //public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto model)
        //{
        //    var result = await _accountService.ResetPassword(model);
        //    return Ok(result);

        //}

        //[HttpGet("Verify-Email")]
        //public async Task<IActionResult> VerifyEmail(string token, string email)
        //{
        //    var verifyEmailResult = await _accountService.ConfirmEmail(token, email);
        //    return Ok(verifyEmailResult);
        //}

        //[HttpPost("SendConfirmationEmail")]
        //public async Task<IActionResult> SendConfirmationEmail(ResendEmailDto resendEmailDto)
        //{
        //    var result = await _accountService.SendConfirmationEmail(resendEmailDto.Email);
        //    return Ok(result);
        //}


        //#endregion

        //#region GetUserProfile
        //[HttpGet("me")]
        //[Authorize]
        //public async Task<IActionResult> GetUser()
        //{
        //    var user = await _accountService.GetUser();
        //    return Ok(user);

        //}

        //#endregion

        //#region Update User

        //[HttpPut("UpdateUser")]
        //[Authorize]
        //public async Task<IActionResult> UpdateUser(UserInfoDto userInfo)
        //{
        //    var result = await _accountService.PutUserAsync(userInfo);
        //    return Ok(result);
        //}

        //[HttpPost("UploadProfilePicture")]
        //[Authorize]
        //public async Task<IActionResult> UploadProfilePicture(UploadProfilePictureDto upload)
        //{
        //    var result = await _imageService.SaveImageAsync(upload);
        //    return Ok(result);
        //}

        //#endregion
    }
}
