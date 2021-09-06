using System;
using System.Threading.Tasks;
using ForecastAPI.Data.Entities;

namespace ForecastAPI.Repositories.Interfaces
{
    public interface IUserRepository: IBaseRepository<User>
    {
        Task<User> GetByEmailAsync(string email);
        bool CheckUserExistsByEmail(string email);
        Task<User> GetInstanceByIdAsync(Guid Id);

        bool CheckUserExistsById(Guid Id);
    }
}