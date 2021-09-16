using System;
using System.Threading.Tasks;
using ForecastAPI.Data.Entities;

namespace ForecastAPI.Repositories.Interfaces
{
    public interface IRegistrationConfirmRepository: IBaseRepository<RegisterConfirm>
    {
        Task<Guid?> RegisterConfirmation(string registerToken);
    }
}