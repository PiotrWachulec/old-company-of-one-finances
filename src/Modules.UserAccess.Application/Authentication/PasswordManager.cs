using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Modules.UserAccess.Application.Authentication
{
    public static class PasswordManager
    {
        public static string HashPassword(string password)
        {
            using (var hmac = new HMACSHA512())
            {
                var salt = Convert.ToBase64String(hmac.Key);

                var hashedPassword = Convert.ToBase64String(
                    hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));

                return $"{salt}.{hashedPassword}";
            }
        }

        public static bool VerifyHashedPassword(string hashedPassword, string passwordToVerify)
        {
            var parts = hashedPassword.Split('.', 2);
            var salt = Convert.FromBase64String(parts[1]);
            var storedHashedPassword = Convert.FromBase64String(parts[0]);

            using (var hmac = new HMACSHA512(salt))
            {
                var passToCheck = hmac.ComputeHash(Encoding.UTF8.GetBytes(passwordToVerify));

                return passToCheck.SequenceEqual(storedHashedPassword);
            }
        }
    }
}