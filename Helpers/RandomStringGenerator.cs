using System;
using System.Security.Cryptography;

namespace ForecastAPI.Helpers
{
    public class RandomStringGenerator
    {
        public static string Generate(int length = 64)
        {
            var randomBytes = new byte[length];
            new RNGCryptoServiceProvider().GetBytes(randomBytes);
            var key = new byte[length + 16];
            Buffer.BlockCopy(randomBytes, 0, key, 0, length);
            Buffer.BlockCopy(Guid.NewGuid().ToByteArray(), 0, key, length, 16);
            return Convert.ToBase64String(key).Replace("=", "").Replace("+", "").Replace("/", "");
        }
    }
}