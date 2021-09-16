using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading.Tasks;
using FluentEmail.Core;
using FluentEmail.Core.Models;
using ForecastAPI.Data.Dtos;
using ForecastAPI.Data.Entities;
using ForecastAPI.Emailing.Models;
using ForecastAPI.Emailing.Services.Interfaces;
using ForecastAPI.Emailing.Settings;
using ForecastAPI.Helpers;
using ForecastAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ForecastAPI.Emailing.Services.Implementations
{
    public class EmailService: IEmailService
    {
        private readonly IFluentEmail _fluentEmail;
        private readonly FluentEmailSettings _fluentEmailSettings;
        private readonly IRegistrationConfirmRepository _registerConfirmRepository;
        private readonly IVerifyPasswordRepository _verifyPasswordRepository;
        public EmailService(IFluentEmail fluentEmail, FluentEmailSettings fluentEmailSettings, IRegistrationConfirmRepository registerConfirmRepository, IVerifyPasswordRepository verifyPasswordRepository)
        {
            _fluentEmail = fluentEmail;
            _fluentEmailSettings = fluentEmailSettings;
            _registerConfirmRepository = registerConfirmRepository;
            _verifyPasswordRepository = verifyPasswordRepository;
        }

        public async Task<SendResponse> SendRegistrationEmailAsync(UserForMailDto userForMail)
        {
            // Generating the random string, which has the purpose of confirming the specific user
            var randomConfirmingString = RandomStringGenerator.Generate();
            var letter = new RegistrationLetter(userForMail.Name, randomConfirmingString);

            // Create temporary registration token for unconfirmed user
            var temporaryRegisterConfirmToken = new RegisterConfirm
            {
                Token = letter.VerifyLink,
                UserId = userForMail.Id
            };
            await _registerConfirmRepository.CreateAsync(temporaryRegisterConfirmToken);
            await _registerConfirmRepository.SaveChangesAsync();
            
            // Sending the email with verifying token to confirm the registration
            return await _fluentEmail
                .To(userForMail.Email)
                .Subject("Registration confirm")
                .UsingTemplateFromEmbedded("ForecastAPI.Views.RegistrationLetter.cshtml", letter, Assembly.GetAssembly(typeof(Startup)))
                .SendAsync();;
        }

        public async Task<SendResponse> SendChangingPasswordEmailAsync(UserForMailDto userForMail)
        {
            var randomVerifyingString = RandomStringGenerator.Generate();
            var forgotLetter = new ForgotLetter(randomVerifyingString);

            var tokenInstance = await _verifyPasswordRepository.GetByCondition(
                    x => x.UserId.Equals(userForMail.Id))
                .SingleOrDefaultAsync();

            if (tokenInstance != null)
            {
                tokenInstance.Token = forgotLetter.VerifyToken;
                tokenInstance.UserId = userForMail.Id;
                tokenInstance.ExpiryTime = DateTime.UtcNow.AddDays(1);
                _verifyPasswordRepository.Update(tokenInstance);
            }
            else
            {
                tokenInstance = new VerifyPassword
                {
                    Token = forgotLetter.VerifyToken,
                    UserId = userForMail.Id,
                    ExpiryTime = DateTime.UtcNow.AddDays(1)
                };
                await _verifyPasswordRepository.CreateAsync(tokenInstance);
            }

            await _verifyPasswordRepository.SaveChangesAsync();
            
            return await _fluentEmail
                .To(userForMail.Email)
                .Subject("Security Verification")
                .UsingTemplateFromEmbedded("ForecastAPI.Views.ChangingPasswordLetter.cshtml", forgotLetter, Assembly.GetAssembly(typeof(Startup)))
                .SendAsync();;
        }
    }
}