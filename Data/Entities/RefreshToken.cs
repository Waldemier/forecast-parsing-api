using System;

namespace ForecastAPI.Data.Entities
{
    public class RefreshToken
    {
        public Guid Id { get; set; }
        public string Token { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public DateTime ExpiryTime { get; set; }
    }
}