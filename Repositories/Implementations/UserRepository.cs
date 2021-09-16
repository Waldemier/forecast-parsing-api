using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ForecastAPI.Data;
using ForecastAPI.Data.Common.Pagination;
using ForecastAPI.Data.Dtos;
using ForecastAPI.Data.Entities;
using ForecastAPI.Data.Enums;
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

        public async Task ChangeRoleById(Guid userId, RoleTypes role)
        {
            var userInstance = await GetByCondition(x => x.Id.Equals(userId)).SingleOrDefaultAsync();
            userInstance.Role = role;
        }

        public async Task<User> GetByEmail(string email) =>
            await GetByCondition(x => x.Email.Equals(email)).SingleOrDefaultAsync();
        
        public bool CheckUserExistsByEmail(string email) =>
            CheckByCondition(x => x.Email.Equals(email));

        public async Task<PagedList<User>> GetAllUsers(UsersRequestPaginationParameters usersRequestPaginationParameters)
        {
            var users =  await GetAll().ToListAsync();
            return PagedList<User>.ToPagedList(users, usersRequestPaginationParameters.PageNumber, usersRequestPaginationParameters.PageSize);
        }

        public bool CheckUserExistsById(Guid Id) =>
            CheckByCondition(x => x.Id.Equals(Id));
        
        public async Task<User> GetInstanceById(Guid Id) =>
            await GetByCondition(x => x.Id.Equals(Id)).SingleOrDefaultAsync();

        public async Task CreateANewUserInstance(User user) =>
            await CreateAsync(user);
        
        /// <summary>
        /// Load history by explicit loading approach
        /// </summary>
        /// <param name="user">Instance type of User</param>
        public async Task LoadHistoryForSpecificUser(User user) =>
            await _context.Entry(user)
                .Collection(c => c.History)
                .Query()
                .LoadAsync();

        public async Task LoadRegisterConfirmTokenForSpecificUserAsync(User user) =>
            await _context.Entry(user)
                .Reference(x => x.RegisterConfirm)
                .LoadAsync();
        
        public async Task LoadVerifyPasswordTokenForSpecificUserAsync(User user) =>
            await _context.Entry(user)
                .Reference(x => x.VerifyPassword)
                .LoadAsync();
        
        public void DeleteUser(User user) => Remove(user);

        public bool CheckIfUserIsConfirmed(Guid userId) =>
            CheckByCondition(u => u.Id.Equals(userId) && !u.Role.Equals(RoleTypes.UnconfirmedUser));
    }
}