using System;
using System.Threading.Tasks;
using ForecastAPI.Data.Entities;

namespace ForecastAPI.Services.Interfaces
{
    public interface ITemporaryTokensService
    {
        Task<Guid?> ConfirmTheRegistration(string registerToken);
        Task<Guid?> VerifyForgotPasswordToken(string verifyToken);

        Task<VerifyPassword> CreateShorterVerificationToken(Guid userId);
        Task<Guid?> VerifyShorterVerificationToken(string shorterVerificationToken);

    }
}