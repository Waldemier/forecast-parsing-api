using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ForecastAPI.Data;
using ForecastAPI.Data.Common.Pagination;
using ForecastAPI.Data.Dtos;
using ForecastAPI.Data.Entities;
using ForecastAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ForecastAPI.Repositories.Implementations
{
    public class UserRepository: BaseRepository<User>, IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        
        public async Task<User> GetByEmailAsync(string email) =>
            await GetByCondition(x => x.Email.Equals(email)).SingleOrDefaultAsync();
        
        public bool CheckUserExistsByEmail(string email) =>
            CheckByCondition(x => x.Email.Equals(email));

        public async Task<PagedList<User>> GetAllUsers(UsersRequestParameters usersRequestParameters)
        {
            var users =  await GetAll().ToListAsync();
            return PagedList<User>.ToPagedList(users, usersRequestParameters.PageNumber, usersRequestParameters.PageSize);
        }

        public bool CheckUserExistsById(Guid Id) =>
            CheckByCondition(x => x.Id.Equals(Id));
        
        public async Task<User> GetInstanceByIdAsync(Guid Id) =>
            await GetByCondition(x => x.Id.Equals(Id)).SingleOrDefaultAsync();

        public async Task CreateANewUserInstance(User user) =>
            await CreateAsync(user);
        
        /// <summary>
        /// Load history by explicit loading approach
        /// </summary>
        /// <param name="user">Instance type of User</param>
        public async Task LoadHistoryForSpecificUserAsync(User user) =>
            await _context.Entry(user)
                .Collection(c => c.History)
                .Query()
                .LoadAsync();

        public void DeleteUser(User user) => Remove(user);
    }
}