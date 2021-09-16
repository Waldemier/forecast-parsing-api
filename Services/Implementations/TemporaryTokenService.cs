using System;
using System.Threading.Tasks;
using ForecastAPI.Data.Dtos;
using ForecastAPI.Data.Entities;
using ForecastAPI.Repositories.Interfaces;
using ForecastAPI.Services.Interfaces;

namespace ForecastAPI.Services.Implementations
{
    public class TemporaryTokenService : ITemporaryTokensService
    {
        private readonly IRegistrationConfirmRepository _registerConfirmRepository;
        private readonly IVerifyPasswordRepository _verifyPasswordRepository;

        public TemporaryTokenService(IRegistrationConfirmRepository registerConfirmRepository, IVerifyPasswordRepository verifyPasswordRepository)
        {
            _registerConfirmRepository = registerConfirmRepository;
            _verifyPasswordRepository = verifyPasswordRepository;
        }
        
        public async Task<Guid?> ConfirmTheRegistration(string registerToken) => 
            await _registerConfirmRepository.RegisterConfirmation(registerToken);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="verifyToken"></param>
        /// <returns>Returns user id, if verification token hasn't expired, it will give to understand that do you have a permission to change the password, or not</returns>
        public async Task<Guid?> VerifyForgotPasswordToken(string verifyToken) =>
            await _verifyPasswordRepository.VerifyForgotPasswordToken(verifyToken);

        public async Task<VerifyPassword> CreateShorterVerificationToken(Guid userId) =>
            await _verifyPasswordRepository.CreateShorterChangingPasswordToken(userId);

        public async Task<Guid?> VerifyShorterVerificationToken(string shorterVerificationToken) =>
            await _verifyPasswordRepository.VerifyShorterVerificationToken(shorterVerificationToken);
    }
}