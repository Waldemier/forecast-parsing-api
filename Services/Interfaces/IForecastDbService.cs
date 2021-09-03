using System.Collections.Generic;
using System.Threading.Tasks;
using ForecastAPI.Data;
using ForecastAPI.Data.Dtos;
using ForecastAPI.Data.Entities;

namespace ForecastAPI.Services
{
    public interface IForecastDbService
    {
        Task<IEnumerable<History>> GetHistoryAsync(RequestForHistoryDto requestForHistoryDto);
        Task SaveToHistoryAsync(FetchForecast fetchedForecast);
    }
}