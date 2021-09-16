using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ForecastAPI.Data;
using ForecastAPI.Data.Dtos;
using ForecastAPI.Data.Entities;
using ForecastAPI.Data.Enums;
using ForecastAPI.Handlers;
using ForecastAPI.Security.Extensions;
using ForecastAPI.Security.Services.Interfaces;
using ForecastAPI.Services;
using ForecastAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ForecastAPI.Controllers
{
    [ApiController, Authorize]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IForecastApiService _forecastApiService;
        private readonly IForecastDbService _forecastDbService;
        private readonly IUserService _userService;
        private readonly IClaimsService _claimsService;
        public WeatherForecastController(ILogger<WeatherForecastController> logger, IForecastApiService forecastApiService, IForecastDbService forecastDbService, IClaimsService claimsService, IUserService userService)
        {
            _logger = logger;
            _forecastApiService = forecastApiService;
            _forecastDbService = forecastDbService;
            _claimsService = claimsService;
            _userService = userService;
        }

        [HttpGet("forecast"), CustomAuthorizeAttribute(RoleTypes.UnconfirmedUser, RoleTypes.SystemUser, RoleTypes.Admin), ServiceFilter(typeof(ValidationRequestFilter))]
        public async Task<ActionResult<FetchForecast>> Get([FromQuery] RequestDto request)
        {
            FetchForecast fetchedForecast = await _forecastApiService.GetWeatherAsync(request);
            Guid currentUserId = _claimsService.GetUserIdFromContextClaims();
            
            // If user didn't confirm the email then the history won't be saved
            if (_userService.CheckIfUserIsConfirmed(currentUserId))
                await _forecastDbService.SaveToHistoryAsync(fetchedForecast, currentUserId);
            
            return Ok(fetchedForecast);
        }

        [HttpGet("history"), CustomAuthorizeAttribute(RoleTypes.SystemUser, RoleTypes.Admin), ServiceFilter(typeof(ValidationRequestFilter))]
        public async Task<ActionResult<IEnumerable<History>>> GetHistory(
            [FromQuery] RequestForHistoryDto requestForHistoryDto) => 
            Ok(await _forecastDbService.GetHistoryForSpecificUser(requestForHistoryDto, _claimsService.GetUserIdFromContextClaims()));
        
    }
}