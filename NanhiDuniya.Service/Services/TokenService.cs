using AutoMapper;
using Azure;
using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NanhiDuniya.Core.Interfaces;
using NanhiDuniya.Core.Models;
using NanhiDuniya.Core.Models.Exceptions;
using NanhiDuniya.Core.Resources.AccountDtos;
using NanhiDuniya.Core.Resources.ResponseDtos;
using NanhiDuniya.Core.Utilities;
using NanhiDuniya.Data.Entities;
using Newtonsoft.Json.Linq;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Numerics;
using System.Runtime;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace NanhiDuniya.Service.Services
{
    public class TokenService : ITokenService
    {
        #region Global declarations 
        private readonly JWTService _jwtService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<TokenService> _logger;
        private readonly ITokenRepository _tokenRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly JwtSettings _settings;
        public TokenService(
            JwtSettings settings,
            UserManager<ApplicationUser> userManager,
            IOptions<JWTService> options,
            ILogger<TokenService> logger,
            ITokenRepository tokenRepository,
            IHttpContextAccessor httpContextAccessor
            )
        {
            _settings = settings;
            _userManager = userManager;
            _logger = logger;
            _jwtService = options.Value;
            _httpContextAccessor = httpContextAccessor;
            _tokenRepository = tokenRepository;
        }
        #endregion
        public async Task<string> GenerateRefreshToken()
        {
            var refreshToken = GenerateRandomString();

            return refreshToken;
        }

        public async Task<string> GenerateConfirmEmailToken(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return null;
            }
            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        public async Task<string> GenerateResetPasswordToken(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return null;
            }
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }
        private string GenerateRandomString()
        {
            var randomBytes = new byte[64];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
                return Convert.ToBase64String(randomBytes);
            }
        }

        public async Task<string> GenerateAccessToken(string email, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = roles.Select(x => new Claim(ClaimTypes.Role, x)).ToList();
            var userClaims = await _userManager.GetClaimsAsync(user);
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, userId)
            }.Union(userClaims).Union(roleClaims);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
            _settings.Issuer,
                _settings.Audience,
            claims,
                expires: DateTime.UtcNow + _settings.Expire,
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<ApiResponse<object>> VerifyRefreshToken()
        {
            if (!(_httpContextAccessor.HttpContext.Request.Cookies.TryGetValue("X-Username", out var userName) && _httpContextAccessor.HttpContext.Request.Cookies.TryGetValue("X-Refresh-Token", out var refreshToken)))
                return new ApiResponse<object>(false, "Missing authentication information. Please log in again.", null, StatusCodes.Status400BadRequest, null);
            var storedToken = await _tokenRepository.GetRefreshTokenAsync(refreshToken);
            if (storedToken == null || storedToken.RefreshToken != refreshToken || storedToken.Expires < DateTime.UtcNow || storedToken.IsRevoked)
            {
                return new ApiResponse<object>(false, "Invalid or expired Refreshtoken. Please log in again.", null, StatusCodes.Status404NotFound, null);
            }
            var accessToken = await GenerateAccessToken(userName, storedToken.UserId);
            _httpContextAccessor.HttpContext.Response.Cookies.Append("X-Access-Token", accessToken, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict, Expires = DateTime.Now.AddMinutes(Convert.ToInt32(_jwtService.AccessTokenExpiry)) });
            return new ApiResponse<object>(true, "Token refreshed successfully.", null, StatusCodes.Status200OK, null);
        }
        public async Task<ApiResponse<object>> RevokeRefreshToken(string userId)
        {
            var tokensToRevoke = await _tokenRepository.GetListOfRefreshTokensByUserIdAsync(userId);
            if (tokensToRevoke == null || tokensToRevoke.Count == 0)
            {
                return new ApiResponse<object>(false, "No refresh tokens found for user.", null, StatusCodes.Status404NotFound, null);
            }

            foreach (var token in tokensToRevoke)
            {
                token.IsRevoked = true;
            }
            await _tokenRepository.UpdateRefreshTokenAsync(tokensToRevoke);
            _httpContextAccessor.HttpContext.Response.Cookies.Delete("X-Access-Token", new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });
            _httpContextAccessor.HttpContext.Response.Cookies.Delete("X-Username", new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });
            _httpContextAccessor.HttpContext.Response.Cookies.Delete("X-Refresh-Token", new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });
            return new ApiResponse<object>(true, "Logout Succesfully Completed.", null, StatusCodes.Status200OK, null);
        }

        public async Task AddRefreshTokenAsync(UserRefreshToken refreshToken)
        {

            await _tokenRepository.AddRefreshTokenAsync(refreshToken);
        }

        public async Task DeleteRefreshTokenAsync(UserRefreshToken refreshToken)
        {
            await _tokenRepository.DeleteRefreshTokenAsync(refreshToken);
        }

        private async Task<ApplicationUser?> FindUserById(string? userId)
        {
            // Logic to find a user by email using the user manager
            return await _userManager.FindByIdAsync(userId!);
        }

        public bool HasTokenExpired(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return true;
            }

            var jwtToken = new JwtSecurityTokenHandler();
            var decodedToken = jwtToken.ReadJwtToken(token);
            var currentTime = DateTime.UtcNow;
            var expirationTime = decodedToken.ValidTo;

            // Check if the token has expired
            return expirationTime < currentTime;
        }
    }
}
