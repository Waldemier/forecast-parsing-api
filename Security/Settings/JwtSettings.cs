using System;

namespace ForecastAPI.Security.Settings
{
    public sealed class JwtSettings
    {
        public string ValidIssuer { get; set; }
        public string ValidAudience { get; set; }
        public string Secret { get; set; }
        public double ExpiryInMinutes { get; set; }
        public double RefreshTokenExpiryInMinutes { get; set; }
    }
}