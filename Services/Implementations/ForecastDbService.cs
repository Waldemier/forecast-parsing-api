using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ForecastAPI.Data;
using ForecastAPI.Data.Dtos;
using ForecastAPI.Data.Entities;
using ForecastAPI.Extensions;
using ForecastAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ForecastAPI.Services.Implementations
{
    public class ForecastDbService: IForecastDbService
    {
        private readonly IHistoryRepository _historyRepository;
        private readonly ILogger<ForecastApiService> _logger;

        public ForecastDbService(IHistoryRepository historyRepository, ILogger<ForecastApiService> logger)
        {
            _historyRepository = historyRepository;
            _logger = logger;
        }
        
        
        public async Task<IEnumerable<History>> GetHistoryAsync(RequestForHistoryDto requestForHistoryDto)
        {
            return await _historyRepository.GetAll()
                .Filtering(requestForHistoryDto)
                .ToListAsync();
        }

        public async Task SaveToHistoryAsync(FetchForecast fetchedForecast)
        {
            var history = new History(DateTime.Now, fetchedForecast.current.temp_c, fetchedForecast.location.name);
            await _historyRepository.CreateAsync(history);
            await _historyRepository.SaveChangesAsync();
        }
    }
}