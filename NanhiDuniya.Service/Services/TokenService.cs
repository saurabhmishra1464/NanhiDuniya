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

        public async Task<RefreshTokenResponse> VerifyRefreshToken(string refreshToken,string userName)
        {
            var user = await _userManager.FindByEmailAsync(userName);
            var storedToken = await _tokenRepository.GetRefreshTokenAsync(refreshToken);
            if (storedToken == null || storedToken.RefreshToken != refreshToken || storedToken.Expires < DateTime.UtcNow || storedToken.IsRevoked)
            {
                return new RefreshTokenResponse
                {
                    Success = false,
                    Message = "Invalid refresh token"
                };
            }
            var accessToken = await GenerateAccessToken(userName,storedToken.UserId);
            _httpContextAccessor.HttpContext.Response.Cookies.Append("X-Access-Token", accessToken, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict, Expires = DateTime.Now.AddMinutes(Convert.ToInt32(_jwtService.AccessTokenExpiry)) });
            return new RefreshTokenResponse
            {
                Success = true,
                Message = "Token refreshed successfully",
            };
        }
        public async Task<LogoutResponse> RevokeRefreshToken(string userId)
        {
            var tokensToRevoke = await _tokenRepository.GetListOfRefreshTokensByUserIdAsync(userId);
            if (tokensToRevoke == null || tokensToRevoke.Count == 0)
            {
                throw new KeyNotFoundException($"No refresh tokens found for user");
            }

            foreach (var token in tokensToRevoke)
            {
                token.IsRevoked = true;
            }
            await _tokenRepository.UpdateRefreshTokenAsync(tokensToRevoke);
            return new LogoutResponse { Success = true, Message = "Logout successful" };
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

        public  bool HasTokenExpired(string token)
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
