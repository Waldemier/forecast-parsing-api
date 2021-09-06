using System;

namespace ForecastAPI.Security.Models
{
    public class AuthenticationResult
    {
        public string JwtToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}