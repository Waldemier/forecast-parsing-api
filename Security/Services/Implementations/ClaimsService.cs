using System;
using System.Linq;
using System.Security.Claims;
using ForecastAPI.Security.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace ForecastAPI.Security.Services.Implementations
{
    public class ClaimsService: IClaimsService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ClaimsService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid GetUserIdFromContextClaims()
        {
            var userIdFromClaims = _httpContextAccessor?.HttpContext?.User.Claims
                .SingleOrDefault(c => c.Type.Equals("Id"));
            
            if (userIdFromClaims is null)
                throw new SecurityTokenException("Some property in the token claims doesn't exist.");

            bool tryParseUserId = Guid.TryParse(userIdFromClaims.Value, out Guid userId);
            if (!tryParseUserId)
                throw new SecurityTokenException("Something has gone wrong with claims.");

            return userId;
        }
    }
}