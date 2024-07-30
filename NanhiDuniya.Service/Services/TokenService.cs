using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NanhiDuniya.Core.Interfaces;
using NanhiDuniya.Core.Models;
using NanhiDuniya.Data.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace NanhiDuniya.Service.Services
{
    public class TokenService : ITokenService
    {
        #region Global declarations 
        private readonly NanhiDuniyaDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly JWTService _jwtService;
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private readonly ILogger<AccountService> _logger;
        private readonly string _encryptionKey = "YourSecretKey";

        //private const string _loginProvider = "NanhiDuniyaUserManagementAPI";
        //private const string _refreshToken = "RefreshToken";
        public TokenService(NanhiDuniyaDbContext context,
            UserManager<ApplicationUser> userManager,
            IMapper mapper,
            IOptions<JWTService> options,
            RoleManager<IdentityRole> roleManager,
            IEmailClientService emailClient,
            IConfiguration configuration,
            IUserService userService,
            ILogger<AccountService> logger

            )
        {
            _dbContext = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtService = options.Value;
            _mapper = mapper;
            _configuration = configuration;
            _userService = userService;
            this._logger = logger;
        }
        #endregion
        public async Task<string> GenerateRefreshToken(string userId)
        {
            var refreshToken = GenerateRandomString(); // Replace with your random token generation logic
            var encryptedRefreshToken = Encrypt(refreshToken);

            var userRefreshToken = new UserRefreshToken
            {
                UserId = userId,
                RefreshToken = encryptedRefreshToken,
                Expires = DateTime.UtcNow.AddDays(15),
                Created = DateTime.UtcNow
            };

            await _dbContext.UserRefreshTokens.AddAsync(userRefreshToken);
            await _dbContext.SaveChangesAsync();

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

        private static string Encrypt(string plainText)
        {
            var newRefreshTokenKey = GenerateRandomKey(32);
            Environment.SetEnvironmentVariable("REFRESH_TOKEN_SECRET", newRefreshTokenKey);
            var refreshTokenKey = Environment.GetEnvironmentVariable("REFRESH_TOKEN_SECRET");
            byte[] iv = new byte[16];
            byte[] array = Encoding.UTF8.GetBytes(refreshTokenKey);
            using (Aes aes = Aes.Create())
            {
                aes.Key = array;
                aes.IV = iv;
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream,
         encryptor, CryptoStreamMode.Write))

                    {
                        using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }
                    }
                    return Convert.ToBase64String(memoryStream.ToArray());

                }
            }
        }

        public async Task<string> GenerateJWT(IEnumerable<Claim> claims)
        {
            var newJwtKey = GenerateRandomKey(32);
            Environment.SetEnvironmentVariable("JWT_SECRET", newJwtKey);
            var jwtKey = Environment.GetEnvironmentVariable("JWT_SECRET");
            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

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

        private static string GenerateRandomKey(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()_-+=";
            // Include special characters
            byte[] data = new byte[length];
            RandomNumberGenerator.Fill(data);
            var builder = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                builder.Append(chars[data[i] % chars.Length]);
            }
            return builder.ToString();
        }

    }
}
