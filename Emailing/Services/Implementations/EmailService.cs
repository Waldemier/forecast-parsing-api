using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading.Tasks;
using FluentEmail.Core;
using FluentEmail.Core.Models;
using ForecastAPI.Emailing.Models;
using ForecastAPI.Emailing.Services.Interfaces;
using ForecastAPI.Emailing.Settings;

namespace ForecastAPI.Emailing.Services.Implementations
{
    public class EmailService: IEmailService
    {
        private readonly IFluentEmail _fluentEmail;
        private readonly FluentEmailSettings _fluentEmailSettings;
        public EmailService(IFluentEmail fluentEmail, FluentEmailSettings fluentEmailSettings)
        {
            _fluentEmail = fluentEmail;
            _fluentEmailSettings = fluentEmailSettings;
        }

        public async Task<SendResponse> SendRegistrationEmailAsync(string userEmail, string userName)
        {
            var randomVerifyingString = GenerateRandomString();
            var letter = new RegistrationLetter(userName, randomVerifyingString);
                
            return await _fluentEmail
                .To(userEmail)
                .Subject("Registration confirm")
                .UsingTemplateFromEmbedded("ForecastAPI.Views.RegistrationLetter.cshtml", letter, Assembly.GetAssembly(typeof(Startup)))
                .SendAsync();
        }
            

        public Task<SendResponse> SendChangingPasswordEmailAsync(string userEmail)
        {
            throw new System.NotImplementedException();
        }

        private static string GenerateRandomString(int length = 64)
        {
            var randomBytes = new byte[length];
            new RNGCryptoServiceProvider().GetBytes(randomBytes); // filling an array above by random bytes

            var key = new byte[length + 16];
            Buffer.BlockCopy(randomBytes, 0, key, 0, length);
            Buffer.BlockCopy(Guid.NewGuid().ToByteArray(), 0, key, length, 16);
            
            return Convert.ToBase64String(key).Replace("=", "").Replace("+","").Replace("/", "");
        }
    }
}