using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using NanhiDuniya.Services.AuthAPI.Service.IService;
using System.Security.Cryptography;

namespace NanhiDuniya.Services.AuthAPI.Service
{
    public class PasswordService : IPasswordService
    {
        public string HashPassword(string password)
        {
            // Generate a 16-byte salt using a secure random number generator
            byte[] salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // Derive a 32-byte subkey (hash) using PBKDF2 with HMAC-SHA256, 10000 iterations
            byte[] hash = KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA256, 10000, 32);

            // Combine the salt and hash into a single byte array
            byte[] hashBytes = new byte[48];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 32);

            // Return the combined salt and hash as a Base64 string
            return Convert.ToBase64String(hashBytes);
        }

        public bool VerifyPassword(string hashedPassword, string providedPassword)
        {
            // Decode the Base64 string to a byte array
            byte[] hashBytes = Convert.FromBase64String(hashedPassword);

            // Extract the salt from the first 16 bytes
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);

            // Extract the stored hash from the remaining 32 bytes
            byte[] storedHash = new byte[32];
            Array.Copy(hashBytes, 16, storedHash, 0, 32);

            // Hash the provided password with the extracted salt
            byte[] providedHash = KeyDerivation.Pbkdf2(providedPassword, salt, KeyDerivationPrf.HMACSHA256, 10000, 32);

            // Perform a fixed-time comparison between the stored hash and the provided hash
            return FixedTimeEquals(storedHash, providedHash);
        }

        private bool FixedTimeEquals(byte[] a, byte[] b)
        {
            uint diff = (uint)a.Length ^ (uint)b.Length;
            for (int i = 0; i < a.Length && i < b.Length; i++)
            {
                diff |= (uint)(a[i] ^ b[i]);
            }
            return diff == 0;
        }
    }
}
