using System.Threading.Tasks;
using ForecastAPI.Data.Dtos;
using ForecastAPI.Data.Entities;
using ForecastAPI.Data.Enums;
using ForecastAPI.Repositories.Interfaces;
using ForecastAPI.Security.Models;
using ForecastAPI.Security.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ForecastAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class AuthController: ControllerBase
    {
        private readonly ISecurityService _securityService;
        private readonly IUserRepository _userRepository;
        
        public AuthController(ISecurityService securityService, IUserRepository userRepository)
        {
            _securityService = securityService;
            _userRepository = userRepository;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var userExisting = _userRepository.CheckUserExistsByEmail(loginDto.Email);
            if (!userExisting)
                return NotFound("Incorrect wrote data.");
            
            var userInstance = await _userRepository.GetByEmailAsync(loginDto.Email);

            var passwordValid = BCrypt.Net.BCrypt.Verify(loginDto.Password, userInstance.Password);
            if (!passwordValid)
                return BadRequest("Incorrect wrote data.");

            var tokens = await _securityService.Authenticate(userInstance);

            if (tokens == null)
                return Unauthorized();
            
            HttpContext.Response.Cookies.Append(
                "access_token",
                tokens.JwtToken,
                new CookieOptions
                {
                    HttpOnly = false,
                    Secure = true,
                    SameSite = SameSiteMode.Strict
                });
            
            HttpContext.Response.Cookies.Append(
                "refresh_token",
                tokens.RefreshToken,
                new CookieOptions
                {
                    HttpOnly = true, // Cant be used on the client side (js)
                    Secure = true, // ssl only
                    SameSite = SameSiteMode.Strict, // requests with the same site only, which means that the cookies won't be fetched from another domain
                    Expires = tokens.RefreshTokenExpiryTime // lives while cookie won't be expiring
                });

            return Ok(new { message = "You registered successfully." });
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (_userRepository.CheckUserExistsByEmail(registerDto.Email))
                return BadRequest("User with current email already exist.");

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);
            var user = new User
            {
                Name = registerDto.Name,
                Email = registerDto.Email,
                Password = hashedPassword,
                Role = RoleTypes.SystemUser
            };

            await _userRepository.CreateAsync(user);
            await _userRepository.SaveChangesAsync();

            return Created(string.Empty, new { message = "User created successfully." });
        }

        [HttpPost("refresh")]
        // cause the refresh token might have expired by time
        [AllowAnonymous] 
        public async Task<IActionResult> Refresh()
        {
            if (!HttpContext.Request.Cookies.ContainsKey("access_token") ||
                !HttpContext.Request.Cookies.ContainsKey("refresh_token"))
            {
                throw new SecurityTokenException("One of the tokens doesn't exists. Try to go back in the system.");
            }

            var refreshCredentials = new RefreshCredentials
            {
                JwtToken = HttpContext.Request.Cookies["access_token"],
                RefreshToken = HttpContext.Request.Cookies["refresh_token"]
            };

            var tokens = await _securityService.Refresh(refreshCredentials);

            if (tokens == null)
                return Unauthorized();
            
            HttpContext.Response.Cookies.Append(
                "access_token",
                tokens.JwtToken,
                new CookieOptions
                {
                    HttpOnly = false,
                    Secure = true,
                    SameSite = SameSiteMode.Strict
                });
            
            HttpContext.Response.Cookies.Append(
                "refresh_token",
                tokens.RefreshToken,
                new CookieOptions
                {
                    HttpOnly = true, // Cant be used on the client side (js)
                    Secure = true, // ssl only
                    SameSite = SameSiteMode.Strict, // requests with the same site only, which means that the cookies won't be fetched from another domain
                    Expires = tokens.RefreshTokenExpiryTime // lives while cookie won't be expiring
                });

            return Ok(new { meassage = "Tokens refreshed successfully." });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            if(HttpContext.Request.Cookies.ContainsKey("access_token")) 
                HttpContext.Response.Cookies.Delete("access_token");
            if(HttpContext.Request.Cookies.ContainsKey("refresh_token")) 
                HttpContext.Response.Cookies.Delete("refresh_token");

            return Ok();
        }
    }
}