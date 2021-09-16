using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using AutoMapper;
using ForecastAPI.Data.Dtos;
using ForecastAPI.Data.Entities;
using ForecastAPI.Data.Enums;
using ForecastAPI.Emailing.Services.Interfaces;
using ForecastAPI.Handlers;
using ForecastAPI.Helpers;
using ForecastAPI.Repositories.Interfaces;
using ForecastAPI.Security.Models;
using ForecastAPI.Security.Services.Interfaces;
using ForecastAPI.Services;
using ForecastAPI.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace ForecastAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController: ControllerBase
    {
        private readonly ISecurityService _securityService;
        private readonly ITemporaryTokensService _temporaryTokensService;
        private readonly IUserService _userService;
        // private readonly ImmutableDictionary<Guid, InnerShortTimeToken> ChangingPasswordProcessDictionary; 

        public AuthController(ISecurityService securityService, IUserService userService, ITemporaryTokensService temporaryTokensService)
        {
            _securityService = securityService;
            _userService = userService;
            _temporaryTokensService = temporaryTokensService;
            // ChangingPasswordProcessDictionary = new Dictionary<Guid, InnerShortTimeToken>().ToImmutableDictionary();
        }

        [HttpPost("login"), ServiceFilter(typeof(ValidationRequestFilter))]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var userExisting = _userService.CheckUserExistsByEmail(loginDto.Email);

            if (!userExisting)
               return new NotFoundObjectResult("Wrong received data. User with current email doesn't exist.");

            var isPasswordValid = await _userService.VerifyPasswordByUserEmail(loginDto.Email, loginDto.Password);
            
            if (!isPasswordValid)
                return new BadRequestObjectResult("Incorrect wrote data.");
            
            var tokens = await _securityService.Authenticate(loginDto.Email);

            if (tokens == null)
                return Unauthorized();
            
            // Now, an access token stored in localStorage, and at the cookie stored a refresh token only
            HttpContext.Response.Cookies.Append(
                "refresh_token",
                tokens.RefreshToken,
                new CookieOptions
                {
                    HttpOnly = true, // Cant be used on the client side (js)
                    Secure = true, // ssl only / put in *true
                    SameSite = SameSiteMode.Strict, // requests with the same site only, which means that the cookies won't be fetched from another domain
                    Expires = tokens.RefreshTokenExpiryTime // lives while cookie won't be expiring
                });

            var userToResponse = await _userService.GetByEmail(loginDto.Email);
            
            return Ok(new {  message = "You logged successfully.", access_token = tokens.JwtToken, user = JsonConvert.SerializeObject(userToResponse) });
        }

        [HttpPost("register"), ServiceFilter(typeof(ValidationRequestFilter))]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var userExisting = _userService.CheckUserExistsByEmail(registerDto.Email);
            if (userExisting)
            {
                return new BadRequestObjectResult("User with current email already exist.");
            }
            
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);
            var userInstanceToSaveInDb = new User
            {
                Name = registerDto.Name,
                Email = registerDto.Email,
                Password = hashedPassword,
                Role = RoleTypes.UnconfirmedUser
            };
            
            await _userService.CreateUserAndSendEmail(userInstanceToSaveInDb);

            return Created(string.Empty, new { message = "User created successfully." });
        }

        [HttpPost("confirm/{registerToken}")]
        public async Task<IActionResult> RegisterConfirmation(string registerToken)
        {
            var response = await _temporaryTokensService.ConfirmTheRegistration(registerToken);
            
            if (!response.HasValue)
                throw new RegisterConfirmationException("Confirmation token is invalid.");
            
            await _userService.ChangeRoleById(response.Value, RoleTypes.SystemUser);
            return Ok("An account has confirmed successfully");
        }

        [HttpPost("forgot")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotResponse forgotResponse)
        {
            if (!_userService.CheckUserExistsByEmail(forgotResponse.Email))
                return BadRequest("User with current email doesn't exist.");
            
            await _userService.GenerateForgotTokenAndSendEmail(forgotResponse.Email);
            
            return Ok("Forgot password letter has sent successfully");
        }
        
        [HttpPost("verify/{verifyToken}")]
        public async Task<IActionResult> VerifyForgotPasswordToken(string verifyToken)
        {
                var response = await _temporaryTokensService.VerifyForgotPasswordToken(verifyToken);
            
            if (!response.HasValue)
                return BadRequest("Verification token has expired.");

            var shorterToken = await _temporaryTokensService.CreateShorterVerificationToken(response.Value);
            
            HttpContext.Response.Cookies.Append(
                    "shorter_verification_token",
                    shorterToken.Token,
                    new CookieOptions()
                    {
                        SameSite = SameSiteMode.Strict,
                        Secure = true,
                        HttpOnly = true, // Cant be used on the client side (js)
                        Expires = shorterToken.ExpiryTime
                    }
            );
            
            return Ok("Now, you can change the password.");
        }
        
        [HttpPost("forgot/change")]
        public async Task<IActionResult> ChangePassword([FromBody] ForgotPasswordRequest request)
        {
            bool cookieResponse = HttpContext.Request.Cookies.TryGetValue("shorter_verification_token", out string shorterToken);
            
            if (!cookieResponse)
                return BadRequest("Verification token doesn't exist or it has expired.");

            if (request == null)
                return BadRequest("Received data is empty");
            
            var verifyResponse = await _temporaryTokensService.VerifyShorterVerificationToken(shorterToken);

            if (!verifyResponse.HasValue)
                return BadRequest("Shorter verification token has expired.");
            
            bool changedPasswordResponse = await _userService.ChangePassword(verifyResponse.Value, request.NewlyPassword);

            if (!changedPasswordResponse)
                return BadRequest("Received newly password is incorrect");

            return Ok("Password has been changed successfully");
        }
        
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(RefreshDto refreshDto) 
        {
            if (!HttpContext.Request.Cookies.ContainsKey("refresh_token") || refreshDto.ExpiredJwtToken == null)
            {
                throw new SecurityTokenException("One of the tokens doesn't exists. Maybe your refresh token had been expired. Try to go back in the system.");
            }

            var refreshCredentials = new RefreshCredentials
            {
                JwtToken = refreshDto.ExpiredJwtToken,
                RefreshToken = HttpContext.Request.Cookies["refresh_token"]
            };

            var tokens = await _securityService.Refresh(refreshCredentials);

            if (tokens == null)
                return Unauthorized();
            
            // Now, an access token stored in localStorage, and at the cookie stored a refresh token only
            HttpContext.Response.Cookies.Append(
                "refresh_token",
                tokens.RefreshToken,
                new CookieOptions
                {
                    HttpOnly = true, // Cant be used on the client side (js)
                    Secure = true, // ssl only / put in *true
                    SameSite = SameSiteMode.Strict, // requests with the same site only, which means that the cookies won't be fetched from another domain
                    Expires = tokens.RefreshTokenExpiryTime // lives while cookie won't be expiring
                });

            return Ok(new { meassage = "Tokens refreshed successfully.", access_token = tokens.JwtToken });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // How, access_token stored in localStorage and, obviously, we don't need to delete it from cookie
            if(HttpContext.Request.Cookies.ContainsKey("refresh_token")) 
                HttpContext.Response.Cookies.Delete("refresh_token");
            
            // anyway if cookies doesn't exist - "Ok" status will have returned
            return Ok();
        }
    }
}