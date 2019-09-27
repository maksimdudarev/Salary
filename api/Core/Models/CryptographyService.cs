using System;
using System.Security.Cryptography;

namespace MD.Salary.WebApi.Core.Models
{
    public static class CryptographyService
    {
        public static string HashPasswordUsingPBKDF2(string password)
        {
            var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, 32)
            {
                IterationCount = 10000
            };

            byte[] hash = rfc2898DeriveBytes.GetBytes(20);

            byte[] salt = rfc2898DeriveBytes.Salt;

            return Convert.ToBase64String(salt) + "|" + Convert.ToBase64String(hash);
        }
    }
}
