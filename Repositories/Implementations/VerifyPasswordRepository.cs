using System;
using System.Threading.Tasks;
using ForecastAPI.Data;
using ForecastAPI.Data.Entities;
using ForecastAPI.Helpers;
using ForecastAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ForecastAPI.Repositories.Implementations
{
    public class VerifyPasswordRepository: BaseRepository<VerifyPassword>, IVerifyPasswordRepository
    {
        public VerifyPasswordRepository(ApplicationDbContext context) : 
            base(context)
        {
        }
        
        public async Task<Guid?> VerifyForgotPasswordToken(string verifyToken)
        {
            var verifyTokenInstance = await GetByCondition(x => x.Token.Equals(verifyToken))
                .SingleOrDefaultAsync();

            Guid? result = null;
            
            // If token doesn't exists, it means that it has expired.
            if (verifyTokenInstance == null)
                return result;
            
            if (DateTime.UtcNow < verifyTokenInstance.ExpiryTime)
            {
                result = verifyTokenInstance.UserId;
            }

            // delete anyway
            Remove(verifyTokenInstance);
            return result;
        }

        public async Task<VerifyPassword> CreateShorterChangingPasswordToken(Guid userId)
        {
            var verifyToken = await GetByCondition(x => x.UserId.Equals(userId)).SingleOrDefaultAsync();
            var randomString = RandomStringGenerator.Generate();

            if (verifyToken != null)
            {
                verifyToken.Token = randomString;
                verifyToken.ExpiryTime = DateTime.UtcNow.AddMinutes(20);
                Update(verifyToken);
            }
            else
            {
                verifyToken = new VerifyPassword
                {
                    UserId = userId,
                    Token = randomString,
                    ExpiryTime = DateTime.UtcNow.AddMinutes(20)
                };
                await CreateAsync(verifyToken);
            }

            await SaveChangesAsync();
            return verifyToken;
        }

        public async Task<Guid?> VerifyShorterVerificationToken(string shorterVerificationToken) => 
            await VerifyForgotPasswordToken(shorterVerificationToken);
    }
}