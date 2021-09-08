using ForecastAPI.Data.Enums;
using Microsoft.AspNetCore.Authorization;

namespace ForecastAPI.Security.Extensions
{
    public class CustomAuthorizeAttribute: AuthorizeAttribute
    {
        public CustomAuthorizeAttribute(params RoleTypes[] roles)
        {
            Roles = string.Join(",", roles);
        }
    }
}