using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ForecastAPI.Data;
using ForecastAPI.Data.Common.Settings;
using ForecastAPI.Data.Dtos;
using ForecastAPI.Data.Entities;
using ForecastAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ForecastAPI.Services.Implementations
{
    public class ForecastService: IForecastService
    {
        private readonly ForecastSettings _forecastSettings;
        private readonly IHistoryRepository _historyRepository;
        private readonly ILogger<ForecastService> _logger;
        public ForecastService(ForecastSettings forecastSettings, IHistoryRepository historyRepository, ILogger<ForecastService> logger)
        {
            _forecastSettings = forecastSettings;
            _historyRepository = historyRepository;
            _logger = logger;
        }
        
        public async Task<FetchForecast> GetWeatherAsync(RequestDto request)
        {
            string queryStringToApi = $"?key={_forecastSettings.Key}&q={request.City}&days={request.Days}";
                
            using var client = new HttpClient();
            using HttpResponseMessage httpResponseMessage = 
                await client.GetAsync(string.Concat(_forecastSettings.Url, "forecast.json", queryStringToApi));
                
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                using HttpContent content = httpResponseMessage.Content;
                FetchForecast data = await content.ReadAsAsync<FetchForecast>();
                
                // Save to history table
                var history = new History(DateTime.Now, data.current.temp_c, data.location.name);
                await _historyRepository.CreateAsync(history);
                await _historyRepository.SaveChangesAsync();
                
                return data;
            }

            return null;
        }

        public async Task<IEnumerable<History>> GetHistoryAsync()
        {
            try
            {
                return await _historyRepository.GetAll().ToListAsync();
            }
            catch (Exception e)
            {
                _logger.LogError($"Database crushed in {nameof(GetHistoryAsync)} action. Exception: {e.Message}");
                return null;
            }
        }
    }
}