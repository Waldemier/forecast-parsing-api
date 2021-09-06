using System;
using System.Threading.Tasks;
using ForecastAPI.Data;
using ForecastAPI.Data.Entities;
using ForecastAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ForecastAPI.Repositories.Implementations
{
    public class UserRepository: BaseRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }
        
        public async Task<User> GetByEmailAsync(string email) =>
            await GetByCondition(x => x.Email.Equals(email)).SingleOrDefaultAsync();
        
        public bool CheckUserExistsByEmail(string email) =>
            CheckByCondition(x => x.Email.Equals(email));

        public bool CheckUserExistsById(Guid Id) =>
            CheckByCondition(x => x.Id.Equals(Id));
        
        public async Task<User> GetInstanceByIdAsync(Guid Id) =>
            await GetByCondition(x => x.Id.Equals(Id)).SingleOrDefaultAsync();
    }
}