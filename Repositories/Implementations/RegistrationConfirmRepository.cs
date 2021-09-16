using System;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using ForecastAPI.Data;
using ForecastAPI.Data.Dtos;
using ForecastAPI.Data.Entities;
using ForecastAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ForecastAPI.Repositories.Implementations
{
    public class RegistrationConfirmRepository: BaseRepository<RegisterConfirm>, IRegistrationConfirmRepository
    {
        public RegistrationConfirmRepository(ApplicationDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Method that checks if register token is valid, if it isn't then return a null, otherwise user id
        /// </summary>
        /// <param name="registerToken"></param>
        /// <returns></returns>
        public async Task<Guid?> RegisterConfirmation(string registerToken)
        {
            var token = await GetByCondition(x => x.Token.Equals(registerToken)).SingleOrDefaultAsync();
            if (token == null) return null;
            
            var userId = token.UserId;
            Remove(token);
            return userId;
        }
    }
}