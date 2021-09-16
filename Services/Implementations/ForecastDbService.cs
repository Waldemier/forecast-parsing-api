using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ForecastAPI.Data;
using ForecastAPI.Data.Dtos;
using ForecastAPI.Data.Entities;
using ForecastAPI.Extensions;
using ForecastAPI.Repositories.Interfaces;
using ForecastAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ForecastAPI.Services.Implementations
{
    public class ForecastDbService: IForecastDbService
    {
        private readonly IHistoryRepository _historyRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ForecastApiService> _logger;

        public ForecastDbService(IHistoryRepository historyRepository, ILogger<ForecastApiService> logger, IMapper mapper)
        {
            _historyRepository = historyRepository;
            _logger = logger;
            _mapper = mapper;
        }
        
        public async Task<IEnumerable<History>> GetHistoryAsync(RequestForHistoryDto requestForHistoryDto) => 
            await _historyRepository.GetAll()
                .Filtering(requestForHistoryDto)
                .Sorting(requestForHistoryDto.OrderBy)
                .ToListAsync();

        public async Task<IEnumerable<History>> GetHistoryForSpecificUser(RequestForHistoryDto requestForHistoryDto, Guid userId) =>
            await _historyRepository.GetByCondition(x => x.UserId.Equals(userId))
                .Filtering(requestForHistoryDto)
                .Sorting(requestForHistoryDto.OrderBy)
                .ToListAsync();


        public async Task SaveToHistoryAsync(FetchForecast fetchedForecast, Guid userId)
        {
            var history = new History(DateTime.Now, fetchedForecast.current.temp_c, fetchedForecast.location.name, userId);
            await _historyRepository.CreateAsync(history);
            await _historyRepository.SaveChangesAsync();
        }

        public PaginatedHistoryToResponseDto PaginateHistory(IEnumerable<History> history, HistoryRequestPaginationParameters historyRequestPaginationParameters)
        {
            var paginatedHistory = _historyRepository.PaginateHistory(history, historyRequestPaginationParameters);
            var historyDtos = _mapper.Map<List<HistoryToResponseDto>>(paginatedHistory);
            
            return new PaginatedHistoryToResponseDto()
            {
                HistoryToResponseDtos = historyDtos,
                MetaData = paginatedHistory.MetaData
            };;
        }
    }
}