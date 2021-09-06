using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ForecastAPI.Data.Entities;
using ForecastAPI.Repositories.Interfaces;
using ForecastAPI.Security.Models;
using ForecastAPI.Security.Services.Interfaces;
using ForecastAPI.Security.Settings;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Extensions;

using AuthenticateCustom = ForecastAPI.Security.Models.AuthenticationResult;

namespace ForecastAPI.Security.Services.Implementations
{
    public class SecurityService: ISecurityService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUserRepository _userRepository;
        
        public SecurityService(JwtSettings jwtSettings, IRefreshTokenRepository refreshTokenRepository, IUserRepository userRepository)
        {
            _jwtSettings = jwtSettings;
            _refreshTokenRepository = refreshTokenRepository;
            _userRepository = userRepository;
        }
        
        public async Task<AuthenticateCustom> Authenticate(User user)
        {
            var jwtToken = JsonWebTokenGenerator(user);
            var refreshToken = RefreshTokenGenerator();

            var dateUpdate = DateTime.UtcNow.AddMinutes(_jwtSettings.RefreshTokenExpiryInMinutes);
            
            if (_refreshTokenRepository.CheckExistingByUserId(user.Id))
            {
                var refreshTokenModel = await _refreshTokenRepository.GetByUserIdAsync(user.Id);
                refreshTokenModel.Token = refreshToken;
                refreshTokenModel.ExpiryTime = dateUpdate;
                
                _refreshTokenRepository.Update(refreshTokenModel);
            }
            else
            {
                var refreshTokenNewInstance = new RefreshToken()
                {
                    Token = refreshToken,
                    UserId = user.Id, 
                    ExpiryTime = dateUpdate
                };
                
                await _refreshTokenRepository.CreateAsync(refreshTokenNewInstance);
            }
            
            await _refreshTokenRepository.SaveChangesAsync();

            return new AuthenticateCustom
            {
                JwtToken = jwtToken,
                RefreshToken = refreshToken,
                RefreshTokenExpiryTime = dateUpdate
            };
        }
        
        public async Task<AuthenticateCustom> Refresh(RefreshCredentials refreshCredentials)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            
            var claimsPrincipal = tokenHandler.ValidateToken(refreshCredentials.JwtToken,
                new TokenValidationParameters
                {
                    ValidAudience = _jwtSettings.ValidAudience,
                    ValidIssuer = _jwtSettings.ValidIssuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret)),

                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = false
                }, 
                out SecurityToken validatedToken);
            
            var jwtToken = validatedToken as JwtSecurityToken;
            
            if (jwtToken == null || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256))
            {
                throw new SecurityTokenException("Received token is invalid");
            }

            if (!_refreshTokenRepository.CheckTokenExists(refreshCredentials.RefreshToken))
                throw new SecurityTokenException("Received refresh token doesn't exist");

            // Gets the claim named as Id and then parse it into GUID
            var tryParseUserId = Guid.TryParse(claimsPrincipal?.FindFirst("Id")?.Value, out Guid Id);
            
            if (!tryParseUserId)
                throw new SecurityTokenException("Something were wrong while parsing the property from claims.");

            if (!_userRepository.CheckUserExistsById(Id))
                throw new SecurityTokenException("User with current email doesn't exist");
            
            var refreshTokenInstance = await _refreshTokenRepository.GetInstanceByToken(refreshCredentials.RefreshToken);

            if (refreshTokenInstance.UserId != Id)
                throw new SecurityTokenException("Received refresh token doesn't belong to the received user");

            if (refreshTokenInstance.ExpiryTime < DateTime.UtcNow)
            {
                _refreshTokenRepository.Remove(refreshTokenInstance);
                await _refreshTokenRepository.SaveChangesAsync();
                
                throw new SecurityTokenException("Expiration time have expired.");
            }

            var userInstance = await _userRepository.GetInstanceByIdAsync(Id);
            
            return await Authenticate(userInstance, claimsPrincipal.Claims.ToArray());
        }

        private async Task<AuthenticateCustom> Authenticate(User user, Claim[] claims)
        {
            var jwtToken = JsonWebTokenGenerator(user, claims);
            var refreshToken = RefreshTokenGenerator();

            var dateUpdate = DateTime.UtcNow.AddMinutes(_jwtSettings.RefreshTokenExpiryInMinutes);
            
            if (_refreshTokenRepository.CheckExistingByUserId(user.Id))
            {
                var refreshTokenModel = await _refreshTokenRepository.GetByUserIdAsync(user.Id);
                refreshTokenModel.Token = refreshToken;
                refreshTokenModel.ExpiryTime = dateUpdate;
                
                _refreshTokenRepository.Update(refreshTokenModel);
                await _refreshTokenRepository.SaveChangesAsync();
            }
            else
            {
                throw new SecurityTokenException("Refresh token doesn't exist for received user.");
            }
            
            return new AuthenticateCustom
            {
                JwtToken = jwtToken,
                RefreshToken = refreshToken,
                RefreshTokenExpiryTime = dateUpdate
            };
        }
        
        private string JsonWebTokenGenerator(User user, Claim[] claims = null)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtSettings.Secret);
            
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims ?? new Claim[]
                {
                    new Claim("Id", user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role.GetDisplayName())
                }),
                Audience = _jwtSettings.ValidAudience,
                Issuer = _jwtSettings.ValidIssuer,
                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryInMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), 
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            
            return tokenHandler.WriteToken(token);
        }

        private string RefreshTokenGenerator()
        {
            var randomNumber = new byte[32]; // by default like [0,0,0,0,0,0,0,0, ...]
            using var randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(randomNumber); // after that array looks like [34, 178, 117, 114, 114, 87, 9, 4, 40, ...]

            return Convert.ToBase64String(randomNumber); // after that array transforms to string
        }
    }
}