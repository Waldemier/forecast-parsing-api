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
    public class ForecastApiService: IForecastApiService
    {
        private readonly ForecastSettings _forecastSettings;
        private readonly IHistoryRepository _historyRepository;
        private readonly ILogger<ForecastApiService> _logger;
        public ForecastApiService(ForecastSettings forecastSettings, IHistoryRepository historyRepository, ILogger<ForecastApiService> logger)
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
                
                return data;
            }

            return null;
        }
    }
}