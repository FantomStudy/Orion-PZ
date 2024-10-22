using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DB_PZ
{
    public class PasswordHelper
    {
        public static string GenerateSalt(int size = 16)
        {
            byte[] salt = new byte[size];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
                return Convert.ToBase64String(salt);
            }
        }

        // Хеширование пароля
        public static string HashPassword(string password, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                // Конкатенируем соль и пароль
                var saltedPassword = $"{salt}{password}";
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        // Проверка пароля
        public static bool VerifyPassword(string enteredPassword, string storedHash, string storedSalt)
        {
            var hashOfInput = HashPassword(enteredPassword, storedSalt);
            return hashOfInput == storedHash;
        }
    }
}
