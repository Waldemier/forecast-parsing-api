using System;

namespace ForecastAPI.Data.Entities.Common
{
    public abstract class TemporaryToken
    {
        public Guid Id { get; set; }
        public string Token { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}