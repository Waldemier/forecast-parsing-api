using System;
using ForecastAPI.Data.Entities.Common;

namespace ForecastAPI.Data.Entities
{
    /// <summary>
    /// Table that stores verify tokens for changing the users passwords
    /// </summary>
    public class VerifyPassword: TemporaryToken
    {
        public DateTime ExpiryTime { get; set; }
    }
}