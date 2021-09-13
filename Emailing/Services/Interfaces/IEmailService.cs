using System.Threading.Tasks;
using FluentEmail.Core.Models;
using ForecastAPI.Emailing.Models;

namespace ForecastAPI.Emailing.Services.Interfaces
{
    public interface IEmailService
    {
        Task<SendResponse> SendRegistrationEmailAsync(string userEmail);
        Task<SendResponse> SendChangingPasswordEmailAsync(string userEmail);
    }
}