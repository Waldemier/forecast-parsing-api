using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ForecastAPI.Data.Dtos;
using ForecastAPI.Data.Entities;
using ForecastAPI.Data.Enums;
using ForecastAPI.Handlers;
using ForecastAPI.Security.Extensions;
using ForecastAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ForecastAPI.Controllers
{
    [ApiController]
    [CustomAuthorize(RoleTypes.Admin)]
    [Route("api/[controller]")]
    public class AdminController: ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public AdminController(IMapper mapper, IUserService userService)
        {
            _mapper = mapper;
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers([FromQuery]UsersRequestPaginationParameters usersRequestPaginationParameters) // ?PageSize=..&PageNumber=..
        {
            var paginatedUsersToResponseDto = await _userService.GetAllUsers(usersRequestPaginationParameters);
            HttpContext.Response.Headers.Add("X-Pagination", paginatedUsersToResponseDto.MetaData);
            
            var userDtos = _mapper.Map<IEnumerable<UserToResponseDto>>(paginatedUsersToResponseDto.UsersToResponseDtos);

            return Ok(userDtos);
        }

        [HttpGet("{userId:Guid}"), ServiceFilter(typeof(ValidationUserExistingFilter))]
        public async Task<IActionResult> GetHistoryForSpecificUser(Guid userId, [FromQuery] HistoryRequestPaginationParameters historyRequestPaginationParameters)
        {
            var paginatedHistoryToResponseDto = await _userService.LoadHistoryWithPaginationForSpecificUser(userId, historyRequestPaginationParameters);
            
            HttpContext.Response.Headers.Add("X-History-Pagination", paginatedHistoryToResponseDto.MetaData);
            
            return Ok(paginatedHistoryToResponseDto.HistoryToResponseDtos);
        }

        [HttpPost("create"), ServiceFilter(typeof(ValidationRequestFilter))]
        public async Task<IActionResult> CreateANewUser([FromBody] UserToCreateDto userToCreateDto)
        {
            var userExisting = _userService.CheckUserExistsByEmail(userToCreateDto.Email);
            if (userExisting)
               return new BadRequestObjectResult("User with current email already exist.");
            
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(userToCreateDto.Password);
            
            var userInstanceToSaveInDb = new User
            {
                Name = userToCreateDto.Name,
                Email = userToCreateDto.Email,
                Password = hashedPassword,
                Role = userToCreateDto.Role
            };
            
            await _userService.CreateUser(userInstanceToSaveInDb);
            
            return Ok(new { message = "A new user instance created successfully." });
        }
        
        [HttpPut("update/{userId:Guid}"), ServiceFilter(typeof(ValidationUserExistingFilter)), ServiceFilter(typeof(ValidationRequestFilter))]
        public async Task<IActionResult> UpdateExistingUser(Guid userId, [FromBody] UserToUpdateDto userToUpdateDto)
        {
            await _userService.UpdateExistingUser(userId, userToUpdateDto);
            return Ok(new { message = $"User with {userId} updated successfully." });
        }
        
        [HttpDelete("delete/{userId:Guid}"), ServiceFilter(typeof(ValidationUserExistingFilter))]
        public async Task<IActionResult> DeleteExistingUser(Guid userId)
        {
            await _userService.DeleteUser(userId);
            
            return Ok(new { message = $"User with {userId} deleted successfully." });
        }
    }
}