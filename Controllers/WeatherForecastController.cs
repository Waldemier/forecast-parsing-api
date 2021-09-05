using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ForecastAPI.Data;
using ForecastAPI.Data.Dtos;
using ForecastAPI.Data.Entities;
using ForecastAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ForecastAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IForecastApiService _forecastApiService;
        private readonly IForecastDbService _forecastDbService;
        
        public WeatherForecastController(ILogger<WeatherForecastController> logger, IForecastApiService forecastApiService, IForecastDbService forecastDbService)
        {
            _logger = logger;
            _forecastApiService = forecastApiService;
            _forecastDbService = forecastDbService;
        }

        [HttpGet("forecast")]
        public async Task<ActionResult<FetchForecast>> Get([FromQuery] RequestDto request)
        {
            FetchForecast fetchedForecast = await _forecastApiService.GetWeatherAsync(request);
            await _forecastDbService.SaveToHistoryAsync(fetchedForecast);
            return Ok(fetchedForecast);
        }

        [HttpGet("history")]
        public async Task<ActionResult<IEnumerable<History>>> GetHistory([FromQuery] RequestForHistoryDto requestForHistoryDto) =>
            Ok(await _forecastDbService.GetHistoryAsync(requestForHistoryDto));
    }
}