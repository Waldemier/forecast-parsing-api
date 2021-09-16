using System.Threading.Tasks;
using ForecastAPI.Data.Entities;
using ForecastAPI.Security.Models;

using AuthenticateCustom = ForecastAPI.Security.Models.AuthenticationResult;

namespace ForecastAPI.Security.Services.Interfaces
{
    public interface ISecurityService
    {
        Task<AuthenticateCustom> Authenticate(string email);
        Task<AuthenticateCustom> Refresh(RefreshCredentials refreshCredentials);
    }
}