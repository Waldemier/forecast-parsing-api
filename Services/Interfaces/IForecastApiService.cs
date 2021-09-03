using System.Collections.Generic;
using System.Threading.Tasks;
using ForecastAPI.Data;
using ForecastAPI.Data.Dtos;
using ForecastAPI.Data.Entities;

namespace ForecastAPI.Services
{
    public interface IForecastApiService
    {
        Task<FetchForecast> GetWeatherAsync(RequestDto request);
    }
}