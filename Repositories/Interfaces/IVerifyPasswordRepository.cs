using System;
using System.Threading.Tasks;
using ForecastAPI.Data.Entities;

namespace ForecastAPI.Repositories.Interfaces
{
    public interface IVerifyPasswordRepository: IBaseRepository<VerifyPassword>
    {
        Task<Guid?> VerifyForgotPasswordToken(string verifyToken);
        Task<VerifyPassword> CreateShorterChangingPasswordToken(Guid userId);
        Task<Guid?> VerifyShorterVerificationToken(string shorterVerificationToken);
    }
}