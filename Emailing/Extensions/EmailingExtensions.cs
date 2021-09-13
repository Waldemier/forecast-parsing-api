using System.Net;
using System.Net.Mail;
using AutoMapper.Configuration;
using ForecastAPI.Emailing.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace ForecastAPI.Emailing.Extensions
{
    public static class EmailingExtensions
    {
        public static void ConfigureEmailing(this IServiceCollection services, FluentEmailSettings fluentEmailSettings)
        {
            services
                .AddFluentEmail(fluentEmailSettings.From)
                .AddRazorRenderer()
                .AddSmtpSender(new SmtpClient(fluentEmailSettings.SmtpSettings.Host, fluentEmailSettings.SmtpSettings.Port)
                {
                    Credentials = new NetworkCredential(fluentEmailSettings.SmtpSettings.Credentials.User, fluentEmailSettings.SmtpSettings.Credentials.Password),
                    EnableSsl = true
                });
        }
    }
}