using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NanhiDuniya.Core.Interfaces;
using NanhiDuniya.Core.Models;
using NanhiDuniya.Core.Models.Exceptions;
using NanhiDuniya.Data.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
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
        //private readonly IHttpContextAccessor _httpContextAccessor;
        public TokenService(
            UserManager<ApplicationUser> userManager,
            IOptions<JWTService> options,
            ILogger<TokenService> logger,
            ITokenRepository tokenRepository
            //IHttpContextAccessor httpContextAccessor

            )
        {
            _userManager = userManager;
            _logger = logger;
            _jwtService = options.Value;
            _tokenRepository = tokenRepository;
            //_httpContextAccessor = httpContextAccessor;
        }
        #endregion
        public async Task<string> GenerateRefreshToken()
        {
            var refreshToken = GenerateRandomString(); // Replace with your random token generation logic
            //var httpContext = _httpContextAccessor.HttpContext;
            //var cookieOptions = new CookieOptions
            //{
            //    HttpOnly = true,
            //    Path = "/",
            //    Secure = true, // Set to true in production
            //    SameSite = SameSiteMode.Strict
            //};
            //httpContext.Response.Cookies.Append("refreshtoken", refreshToken, cookieOptions);
            
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

        public async Task<string> GenerateAccessToken(string userId)
        {
            var user = await FindUserById(userId);

            if (user == null)
            {
                _logger.LogError("Invalid user or access denied");
                throw new UnauthorizedAccessException();
            }
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = roles.Select(x => new Claim(ClaimTypes.Role, x)).ToList();
            var userClaims = await _userManager.GetClaimsAsync(user);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id),
            }
            .Union(userClaims).Union(roleClaims);
            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtService.Secret!));

            var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtService.Issuer,
                audience: _jwtService.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToInt32(_jwtService.AccessTokenExpiryMinutes)),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<RefreshTokenDto> VerifyRefreshToken(RefreshTokenDto request)
        {
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var tokenContent = jwtSecurityTokenHandler.ReadJwtToken(request.Token);
            var userId = tokenContent.Claims.ToList().FirstOrDefault(q => q.Type == JwtRegisteredClaimNames.Sub)?.Value;

            _logger.LogError("Verifying refresh token for user ID: {UserId}", userId);
            var user = await FindUserById(userId);

            if (user == null || !user.IsActive)
            {
                _logger.LogWarning("Invalid user or access denied");
                throw new UnauthorizedAccessException();
            }

            var storedToken = await _tokenRepository.GetRefreshTokenAsync(userId, request.RefreshToken);
            if (storedToken == null || storedToken.RefreshToken != request.RefreshToken || storedToken.Expires < DateTime.UtcNow || storedToken.IsRevoked)
            {
                throw new UnauthorizedAccessException();
            }
            var accessToken = await GenerateAccessToken(user.Id);

            return new RefreshTokenDto
            {
                Token = accessToken,
                RefreshToken = storedToken.RefreshToken
            };
        }
        public async Task RevokeRefreshToken(string userId)
        {
            var existingRefreshTokens = await _tokenRepository.GetListOfRefreshTokensByUserIdAsync(userId);
            if (existingRefreshTokens == null || existingRefreshTokens.Count == 0)
            {
                throw new KeyNotFoundException();
            }
            foreach (var token in existingRefreshTokens)
            {
                if (token.IsRevoked == false)
                {
                    token.IsRevoked = true;
                    await _tokenRepository.UpdateRefreshTokenAsync(token);
            
                }
            }
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
    }
}
