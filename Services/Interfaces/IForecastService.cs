using System.Threading.Tasks;
using ForecastAPI.Data;
using ForecastAPI.Data.Common.Settings;
using ForecastAPI.Data.Dtos;

namespace ForecastAPI.Services
{
    public interface IForecastService
    {
        Task<FetchForecast> GetWeather(RequestDto request);
    }
}