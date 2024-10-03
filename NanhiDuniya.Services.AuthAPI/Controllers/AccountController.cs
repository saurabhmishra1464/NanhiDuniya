using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NanhiDuniya.Services.AuthAPI.Models.Dto;

namespace NanhiDuniya.Services.AuthAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        #region Global declarations

        public AccountController()
        {

        }
        #endregion

        #region User Registration Endpoint

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDto model)
        {
            var result = await _accountService.Register(model);
            return Ok(result);

        }
        #endregion

        #region Admin Registration Endpoint

        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost("Register-Admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegistrationRequestDto model)
        {
            var result = await _accountService.Register(_mapper.Map<RegisterModel>(model));
            return Ok(result);
        }
        #endregion

        #region Authentication Token and handshake endpoints 

        [HttpPost("login")]
        public async Task<IActionResult> LoginApi([FromBody] LoginRequestDto model)
        {
            var result = await _accountService.Login(_mapper.Map<LoginModel>(model));
            return Ok(result);
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto forgotPasswordDto)
        {
            var resetPassword = await _accountService.ForgotPassword(forgotPasswordDto.Email);

            return Ok(resetPassword);
        }

        [HttpGet("refresh")]
        public async Task<IActionResult> Refresh()
        {
            var result = await _tokenService.VerifyRefreshToken();

            return Ok(result);
        }

        [HttpGet("check-auth")]
        public IActionResult CheckAuth()
        {
            var userClaims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();
            return Ok(new { Success = true, message = "User is authenticated", claims = userClaims });
        }



        [HttpPost("RevokeRefreshToken")]
        [Authorize]
        public async Task<IActionResult> RevokeRefreshToken(RevokeRefreshTokenDto revokeRefreshTokenRequest)
        {
            var result = await _tokenService.RevokeRefreshToken(revokeRefreshTokenRequest.UserId);
            return Ok(result);
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto model)
        {
            var result = await _accountService.ResetPassword(model);
            return Ok(result);

        }

        [HttpGet("Verify-Email")]
        public async Task<IActionResult> VerifyEmail(string token, string email)
        {
            var verifyEmailResult = await _accountService.ConfirmEmail(token, email);
            return Ok(verifyEmailResult);
        }

        [HttpPost("SendConfirmationEmail")]
        public async Task<IActionResult> SendConfirmationEmail(ResendEmailDto resendEmailDto)
        {
            var result = await _accountService.SendConfirmationEmail(resendEmailDto.Email);
            return Ok(result);
        }


        #endregion

        #region GetUserProfile
        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetUser()
        {
            var user = await _accountService.GetUser();
            return Ok(user);

        }

        #endregion

        #region Update User

        [HttpPut("UpdateUser")]
        [Authorize]
        public async Task<IActionResult> UpdateUser(UserInfoDto userInfo)
        {
            var result = await _accountService.PutUserAsync(userInfo);
            return Ok(result);
        }

        [HttpPost("UploadProfilePicture")]
        [Authorize]
        public async Task<IActionResult> UploadProfilePicture(UploadProfilePictureDto upload)
        {
            var result = await _imageService.SaveImageAsync(upload);
            return Ok(result);
        }

        #endregion
    }
}
