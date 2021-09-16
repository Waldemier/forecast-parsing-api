using System;
using System.Threading.Tasks;
using ForecastAPI.Data.Dtos;
using ForecastAPI.Data.Entities;
using ForecastAPI.Data.Enums;

namespace ForecastAPI.Services.Interfaces
{
    public interface IUserService
    {
        Task<bool> ChangePassword(Guid userId, string newlyPassword);
        Task GenerateForgotTokenAndSendEmail(string email);
        Task ChangeRoleById(Guid userId, RoleTypes role);
        Task CreateUserAndSendEmail(User user);
        Task<bool> VerifyPasswordByUserEmail(string email, string password);
        Task<bool> VerifyPasswordByUserId(Guid userId, string password);

        bool CheckIfUserIsConfirmed(Guid userId);
        Task<UserToResponseDto> GetByEmail(string email);
        bool CheckUserExistsByEmail(string email);
        Task UpdateExistingUser(Guid userId, UserToUpdateDto userToUpdateDto);
        Task<PaginatedUsersToResponseDto> GetAllUsers(UsersRequestPaginationParameters usersRequestPaginationParameters);
        bool CheckUserExistsById(Guid userId);
        Task<UserToResponseDto> GetUserById(Guid userId);

        Task<PaginatedHistoryToResponseDto> LoadHistoryWithPaginationForSpecificUser(Guid userId, HistoryRequestPaginationParameters historyRequestPaginationParameters);
        Task CreateUser(User user);
        Task DeleteUser(Guid userId);
    }
}