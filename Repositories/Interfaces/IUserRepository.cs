using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ForecastAPI.Data.Entities;

namespace ForecastAPI.Repositories.Interfaces
{
    public interface IUserRepository: IBaseRepository<User>
    {
        Task<User> GetByEmailAsync(string email);
        bool CheckUserExistsByEmail(string email);
        Task<User> GetInstanceByIdAsync(Guid Id);
        Task<IEnumerable<User>> GetAllUsers();
        bool CheckUserExistsById(Guid Id);
        Task LoadHistoryForSpecificUserAsync(User user);
        Task CreateANewUserInstance(User user);
        void DeleteUser(User user);
    }
}