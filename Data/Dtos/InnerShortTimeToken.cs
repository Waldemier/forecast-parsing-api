using System;

namespace ForecastAPI.Data.Dtos
{
    public class InnerShortTimeToken
    {
        public string Token { get; set; }
        public DateTime ExpiryTime { get; } = DateTime.UtcNow.AddMinutes(20);
    }
}