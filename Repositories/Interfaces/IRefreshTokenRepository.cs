using System;
using System.Threading.Tasks;
using ForecastAPI.Data.Entities;

namespace ForecastAPI.Repositories.Interfaces
{
    public interface IRefreshTokenRepository: IBaseRepository<RefreshToken>
    {
        bool CheckExistingByUserId(Guid userId);
        Task<RefreshToken> GetByUserIdAsync(Guid userId);
        bool CheckTokenExists(string refreshTokenToValidate);
        Task<RefreshToken> GetInstanceByToken(string refreshToken);
    }
}