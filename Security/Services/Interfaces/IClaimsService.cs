using System;

namespace ForecastAPI.Security.Services.Interfaces
{
    public interface IClaimsService
    {
        Guid GetUserIdFromContextClaims();
    }
}