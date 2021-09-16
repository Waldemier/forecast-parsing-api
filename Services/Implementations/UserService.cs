using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ForecastAPI.Data.Dtos;
using ForecastAPI.Data.Entities;
using ForecastAPI.Data.Enums;
using ForecastAPI.Emailing.Services.Interfaces;
using ForecastAPI.Handlers;
using ForecastAPI.Repositories.Interfaces;
using ForecastAPI.Services.Interfaces;

namespace ForecastAPI.Services.Implementations
{
    public class UserService: IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IForecastDbService _forecastDbService;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;
        
        public UserService(IUserRepository userRepository, IMapper mapper, IForecastDbService forecastDbService, IEmailService emailService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _forecastDbService = forecastDbService;
            _emailService = emailService;
        }

        public async Task<bool> ChangePassword(Guid userId, string newlyPassword)
        {
            if (!string.IsNullOrWhiteSpace(newlyPassword))
            {
                var currentUser = await _userRepository.GetInstanceById(userId);
                currentUser.Password = BCrypt.Net.BCrypt.HashPassword(newlyPassword);;
                await _userRepository.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task GenerateForgotTokenAndSendEmail(string email)
        {
            if (!_userRepository.CheckUserExistsByEmail(email))
                throw new ForgotPasswordException("User with current email doesn't exist.");
            
            var user = await _userRepository.GetByEmail(email);
            var userDto = _mapper.Map<UserForMailDto>(user);
            
            await _emailService.SendChangingPasswordEmailAsync(userDto);
        }

        public async Task ChangeRoleById(Guid userId, RoleTypes role)
        {
            await _userRepository.ChangeRoleById(userId, role);
            await _userRepository.SaveChangesAsync();
        }

        public async Task CreateUserAndSendEmail(User user)
        {
            await CreateUser(user).ContinueWith(async _ =>
            {
                var userForLetter = _mapper.Map<UserForMailDto>(user);
                return await _emailService.SendRegistrationEmailAsync(userForLetter);
            }).Unwrap(); // Unwrap extension method represents the logic,
                         // that responsible for works correctly, when async action looks like Task<Task<T>>
        }

        public async Task<bool> VerifyPasswordByUserEmail(string email, string password)
        {
            var userInstance = await _userRepository.GetByEmail(email);
            return BCrypt.Net.BCrypt.Verify(password, userInstance.Password);
        }

        public async Task<bool> VerifyPasswordByUserId(Guid userId, string password)
        {
            var userInstance = await _userRepository.GetInstanceById(userId);
            return BCrypt.Net.BCrypt.Verify(password, userInstance.Password);
        }

        public bool CheckIfUserIsConfirmed(Guid userId) => _userRepository.CheckIfUserIsConfirmed(userId);

        public async Task<UserToResponseDto> GetByEmail(string email)
        {
            var userInstance = await _userRepository.GetByEmail(email);
            var userToResponseDto = _mapper.Map<UserToResponseDto>(userInstance);
            return userToResponseDto;
        }

        public bool CheckUserExistsByEmail(string email) => _userRepository.CheckUserExistsByEmail(email);

        public async Task UpdateExistingUser(Guid userId, UserToUpdateDto userToUpdateDto)
        {
            var userInstance = await _userRepository.GetInstanceById(userId);
            
            if (userInstance.Role == RoleTypes.Admin)
                throw new AdminException("Cannot to change the current properties, because this user have got an admin role.");
            
            // Changed the user instance above
            _mapper.Map(userToUpdateDto, userInstance);
            
            // Saves changes which user has received automatically 
            await _userRepository.SaveChangesAsync();
        }

        public async Task<PaginatedUsersToResponseDto> GetAllUsers(UsersRequestPaginationParameters usersRequestPaginationParameters)
        {
            var paginatedUsers = await _userRepository.GetAllUsers(usersRequestPaginationParameters);
            var paginatedUsersDtos = _mapper.Map<IEnumerable<UserToResponseDto>>(paginatedUsers);

            return new PaginatedUsersToResponseDto
            {
                UsersToResponseDtos = paginatedUsersDtos,
                MetaData = paginatedUsers.MetaData
            };
        }

        public bool CheckUserExistsById(Guid userId) => _userRepository.CheckUserExistsById(userId);
        
        public async Task<UserToResponseDto> GetUserById(Guid userId)
        {
            var userInstance = await _userRepository.GetInstanceById(userId);
            var userToResponseDto = _mapper.Map<UserToResponseDto>(userInstance);
            return userToResponseDto;
        }

        public async Task<PaginatedHistoryToResponseDto> LoadHistoryWithPaginationForSpecificUser(Guid userId, HistoryRequestPaginationParameters historyRequestPaginationParameters)
        {
            var user = await _userRepository.GetInstanceById(userId);
            await _userRepository.LoadHistoryForSpecificUser(user);
            
            var paginatedHistoryToResponseDto = _forecastDbService.PaginateHistory(user.History, historyRequestPaginationParameters);

            return paginatedHistoryToResponseDto;
        }

        public async Task CreateUser(User user)
        {
            await _userRepository.CreateANewUserInstance(user);
            await _userRepository.SaveChangesAsync();
        }

        public async Task DeleteUser(Guid userId)
        {
            var userInstance = await _userRepository.GetInstanceById(userId);
            _userRepository.DeleteUser(userInstance);
            await _userRepository.SaveChangesAsync();
        }
    }
}