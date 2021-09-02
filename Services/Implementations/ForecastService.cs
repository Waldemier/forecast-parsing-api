using System;
using System.Net.Http;
using System.Threading.Tasks;
using ForecastAPI.Data;
using ForecastAPI.Data.Common.Settings;
using ForecastAPI.Data.Dtos;
using Microsoft.Extensions.Configuration;

namespace ForecastAPI.Services.Implementations
{
    public class ForecastService: IForecastService
    {
        private readonly IConfiguration _configuration;
        private readonly ForecastSettings _forecastSettings;

        public ForecastService(IConfiguration configuration)
        {
            _configuration = configuration;
            _forecastSettings = new ForecastSettings();
            _configuration.GetSection("ForecastSettings").Bind(_forecastSettings);
        }
        
        public async Task<FetchForecast> GetWeather(RequestDto request)
        {
            string queryStringToApi = String.Format("?key={0}&q={1}&days={2}", _forecastSettings.Key, request.City, request.Days);
                
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