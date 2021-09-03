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
        private readonly IForecastService _forecastService;
        
        public WeatherForecastController(ILogger<WeatherForecastController> logger, IForecastService forecastService)
        {
            _logger = logger;
            _forecastService = forecastService;
        }

        [HttpGet("forecast")]
        public async Task<ActionResult<FetchForecast>> Get([FromQuery] RequestDto request) => 
            Ok(await _forecastService.GetWeatherAsync(request));

        [HttpGet("history")]
        public async Task<ActionResult<IEnumerable<History>>> GetHistory() =>
            Ok(await _forecastService.GetHistoryAsync());
    }
}