using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ForecastAPI.Data.Dtos;
using ForecastAPI.Data.Entities;
using ForecastAPI.Data.Enums;
using ForecastAPI.Handlers;
using ForecastAPI.Repositories.Interfaces;
using ForecastAPI.Security.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace ForecastAPI.Controllers
{
    [ApiController]
    [CustomAuthorize(RoleTypes.Admin)]
    [Route("api/[controller]")]
    public class AdminController: ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IHistoryRepository _historyRepository;
        public AdminController(IUserRepository userRepository, IMapper mapper, IHistoryRepository historyRepository)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _historyRepository = historyRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers([FromQuery]UsersRequestPaginationParameters usersRequestPaginationParameters) // ?PageSize=..&PageNumber=..
        {
            var users = await _userRepository.GetAllUsers(usersRequestPaginationParameters);
            HttpContext.Response.Headers.Add("X-Pagination", users.MetaData);
            
            var userDtos = _mapper.Map<IEnumerable<UserToResponseDto>>(users);

            return Ok(userDtos);
        }

        [HttpGet("{userId:Guid}"), ServiceFilter(typeof(ValidationUserExistingFilter))]
        public async Task<IActionResult> GetHistoryForSpecificUser(Guid userId, [FromQuery] HistoryRequestPaginationParameters historyRequestPaginationParameters)
        {
            var user = await _userRepository.GetInstanceByIdAsync(userId);
            await _userRepository.LoadHistoryForSpecificUserAsync(user);
            var paginatedHistory = _historyRepository.PaginateHistory(user.History, historyRequestPaginationParameters);
            
            HttpContext.Response.Headers.Add("X-History-Pagination", paginatedHistory.MetaData);
            
            var historyDto = _mapper.Map<IEnumerable<HistoryToResponseDto>>(paginatedHistory);
            
            return Ok(historyDto);
        }

        [HttpPost("create"), ServiceFilter(typeof(ValidationRequestFilter))]
        public async Task<IActionResult> CreateANewUser([FromBody] UserToCreateDto userToCreateDto)
        {
            var userExisting = _userRepository.CheckUserExistsByEmail(userToCreateDto.Email);
            if (userExisting)
            {
               return new BadRequestObjectResult("User with current email already exist.");
            }
            
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(userToCreateDto.Password);
            
            var userInstanceToSaveInDb = new User
            {
                Name = userToCreateDto.Name,
                Email = userToCreateDto.Email,
                Password = hashedPassword,
                Role = userToCreateDto.Role
            };
            
            await _userRepository.CreateANewUserInstance(userInstanceToSaveInDb);
            await _userRepository.SaveChangesAsync();
            
            return Ok(new { message = "A new user instance created successfully." });
        }
        
        [HttpPut("update/{userId:Guid}"), ServiceFilter(typeof(ValidationUserExistingFilter)), ServiceFilter(typeof(ValidationRequestFilter))]
        public async Task<IActionResult> UpdateExistingUser(Guid userId, [FromBody] UserToUpdateDto userToUpdateDto)
        {
            var user = await _userRepository.GetInstanceByIdAsync(userId);
            
            if (user.Role == RoleTypes.Admin)
                throw new CustomAdminException("Cannot to change the current properties, because this user have got an admin role.");
                    
            // Changed the user instance above
            _mapper.Map(userToUpdateDto, user);
            // Saves changes which user has received automatically 
            await _userRepository.SaveChangesAsync();
            
            return Ok(new { message = $"User with {userId} updated successfully." });
        }
        
        [HttpDelete("delete/{userId:Guid}"), ServiceFilter(typeof(ValidationUserExistingFilter))]
        public async Task<IActionResult> DeleteExistingUser(Guid userId)
        {
            var user = await _userRepository.GetInstanceByIdAsync(userId);
            _userRepository.DeleteUser(user);
            await _userRepository.SaveChangesAsync();
            
            return Ok(new { message = $"User with {userId} deleted successfully." });
        }
    }
}