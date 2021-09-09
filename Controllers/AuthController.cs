using System.Threading.Tasks;
using AutoMapper;
using ForecastAPI.Data.Dtos;
using ForecastAPI.Data.Entities;
using ForecastAPI.Data.Enums;
using ForecastAPI.Handlers;
using ForecastAPI.Repositories.Interfaces;
using ForecastAPI.Security.Models;
using ForecastAPI.Security.Services.Interfaces;
using Microsoft.AspNetCore.Http;
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
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        
        public AuthController(ISecurityService securityService, IUserRepository userRepository, IMapper mapper)
        {
            _securityService = securityService;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpPost("login"), ServiceFilter(typeof(ValidationRequestFilter))]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var userExisting = _userRepository.CheckUserExistsByEmail(loginDto.Email);

            if (!userExisting)
            {
               return new NotFoundObjectResult("Wrong received data. User with current email doesn't exist.");
            }

            var userInstance = await _userRepository.GetByEmailAsync(loginDto.Email);
            var passwordValid = BCrypt.Net.BCrypt.Verify(loginDto.Password, userInstance.Password);
            
            if (!passwordValid)
            {
                return new BadRequestObjectResult("Incorrect wrote data.");
            }
                
            var tokens = await _securityService.Authenticate(userInstance);

            if (tokens == null)
                return Unauthorized();
            
            HttpContext.Response.Cookies.Append(
                "access_token",
                tokens.JwtToken,
                new CookieOptions
                {
                    HttpOnly = false,
                    Secure = true, // ssl only / put in *true
                    SameSite = SameSiteMode.Strict
                });
            
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

            var userToResponse = _mapper.Map<UserToResponseDto>(userInstance);
            
            return Ok(new { message = "You logged successfully.", user = JsonConvert.SerializeObject(userToResponse) });
        }

        [HttpPost("register"), ServiceFilter(typeof(ValidationRequestFilter))]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var userExisting = _userRepository.CheckUserExistsByEmail(registerDto.Email);
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
                Role = RoleTypes.SystemUser
            };
                
            await _userRepository.CreateAsync(userInstanceToSaveInDb);
            await _userRepository.SaveChangesAsync();

            return Created(string.Empty, new { message = "User created successfully." });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh()
        {
            if (!HttpContext.Request.Cookies.ContainsKey("access_token") ||
                !HttpContext.Request.Cookies.ContainsKey("refresh_token"))
            {
                throw new SecurityTokenException("One of the tokens doesn't exists. Maybe your refresh token had been expired. Try to go back in the system.");
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
                    Secure = true, // ssl only / put in *true
                    SameSite = SameSiteMode.Strict
                });
            
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

            return Ok(new { meassage = "Tokens refreshed successfully." });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            if(HttpContext.Request.Cookies.ContainsKey("access_token")) 
                HttpContext.Response.Cookies.Delete("access_token");
            if(HttpContext.Request.Cookies.ContainsKey("refresh_token")) 
                HttpContext.Response.Cookies.Delete("refresh_token");
            
            // anyway if cookies doesn't exist - "Ok" status will return
            return Ok();
        }
    }
}