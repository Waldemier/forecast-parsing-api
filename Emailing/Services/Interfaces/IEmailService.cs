using System.Threading.Tasks;
using FluentEmail.Core.Models;
using ForecastAPI.Data.Dtos;

namespace ForecastAPI.Emailing.Services.Interfaces
{
    public interface IEmailService
    {
        Task<SendResponse> SendRegistrationEmailAsync(UserForMailDto userForMail);
        Task<SendResponse> SendChangingPasswordEmailAsync(UserForMailDto userForMailDto);
    }
}