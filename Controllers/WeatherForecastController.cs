using System.Collections.Generic;
using System.Threading.Tasks;
using ForecastAPI.ActionFilters;
using ForecastAPI.Data;
using ForecastAPI.Data.Dtos;
using ForecastAPI.Data.Entities;
using ForecastAPI.Data.Enums;
using ForecastAPI.Security.Extensions;
using ForecastAPI.Security.Services.Interfaces;
using ForecastAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Extensions;

namespace ForecastAPI.Controllers
{
    [ApiController, Authorize]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IForecastApiService _forecastApiService;
        private readonly IForecastDbService _forecastDbService;
        private readonly IClaimsService _claimsService;
        public WeatherForecastController(ILogger<WeatherForecastController> logger, IForecastApiService forecastApiService, IForecastDbService forecastDbService, IClaimsService claimsService)
        {
            _logger = logger;
            _forecastApiService = forecastApiService;
            _forecastDbService = forecastDbService;
            _claimsService = claimsService;
        }

        [HttpGet("forecast"), CustomAuthorizeAttribute(RoleTypes.SystemUser, RoleTypes.Admin), ServiceFilter(typeof(ValidationRequestsFilter))]
        public async Task<ActionResult<FetchForecast>> Get([FromQuery] RequestDto request)
        {
            FetchForecast fetchedForecast = await _forecastApiService.GetWeatherAsync(request);
            await _forecastDbService.SaveToHistoryAsync(fetchedForecast, _claimsService.GetUserIdFromContextClaims());
            return Ok(fetchedForecast);
        }

        [HttpGet("history"), CustomAuthorizeAttribute(RoleTypes.SystemUser, RoleTypes.Admin), ServiceFilter(typeof(ValidationRequestsFilter))]
        public async Task<ActionResult<IEnumerable<History>>> GetHistory(
            [FromQuery] RequestForHistoryDto requestForHistoryDto) => 
            Ok(await _forecastDbService.GetHistoryForSpecificUser(requestForHistoryDto, _claimsService.GetUserIdFromContextClaims()));
        
    }
}