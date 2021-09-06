using System;
using System.Text;
using ForecastAPI.Security.Services.Implementations;
using ForecastAPI.Security.Services.Interfaces;
using ForecastAPI.Security.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace ForecastAPI.Security.Extensions
{
    internal static class SecurityExtensions
    {
        public static void AddSecurityServices(this IServiceCollection services)
        {
            services.AddScoped<ISecurityService, SecurityService>();
        }
        
        public static void ConfigureJsonWebToken(this IServiceCollection services, JwtSettings jwtSettings)
        {
            // To ask about ChallengeScheme
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                // if false then SSL won't be using while requesting
                options.RequireHttpsMetadata = true; 
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    
                    ValidAudience = jwtSettings.ValidAudience,
                    ValidIssuer = jwtSettings.ValidIssuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
                };
            });
        }
    }
}