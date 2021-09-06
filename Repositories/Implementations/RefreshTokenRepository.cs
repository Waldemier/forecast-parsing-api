using System;
using System.Threading.Tasks;
using ForecastAPI.Data;
using ForecastAPI.Data.Entities;
using ForecastAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ForecastAPI.Repositories.Implementations
{
    public class RefreshTokenRepository: BaseRepository<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(ApplicationDbContext context) : base(context)
        {
        }

        public bool CheckExistingByUserId(Guid userId) =>
            CheckByCondition(x => x.UserId.Equals(userId));

        public async Task<RefreshToken> GetByUserIdAsync(Guid userId) =>
            await GetByCondition(x => x.UserId.Equals(userId)).SingleOrDefaultAsync();

        public bool CheckTokenExists(string refreshTokenToValidate) =>
            CheckByCondition(x => x.Token.Equals(refreshTokenToValidate));

        public async Task<RefreshToken> GetInstanceByToken(string refreshToken) =>
            await GetByCondition(x => x.Token.Equals(refreshToken)).SingleOrDefaultAsync();
    }
}