using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ForecastAPI.Data.Common.Pagination;
using ForecastAPI.Data.Dtos;
using ForecastAPI.Data.Entities;
using ForecastAPI.Data.Enums;

namespace ForecastAPI.Repositories.Interfaces
{
    public interface IUserRepository: IBaseRepository<User>
    {
        Task ChangeRoleById(Guid userId, RoleTypes role);
        Task<User> GetByEmail(string email);
        bool CheckUserExistsByEmail(string email);
        Task<User> GetInstanceById(Guid Id);
        Task<PagedList<User>> GetAllUsers(UsersRequestPaginationParameters usersRequestPaginationParameters);
        bool CheckUserExistsById(Guid Id);
        Task LoadHistoryForSpecificUser(User user);
        Task CreateANewUserInstance(User user);
        void DeleteUser(User user);
        bool CheckIfUserIsConfirmed(Guid Id);
    }
}