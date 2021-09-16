using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ForecastAPI.Data;
using ForecastAPI.Data.Dtos;
using ForecastAPI.Data.Entities;

namespace ForecastAPI.Services.Interfaces
{
    public interface IForecastDbService
    {
        Task<IEnumerable<History>> GetHistoryAsync(RequestForHistoryDto requestForHistoryDto);
        Task<IEnumerable<History>> GetHistoryForSpecificUser(RequestForHistoryDto requestForHistoryDto, Guid userId);
        Task SaveToHistoryAsync(FetchForecast fetchedForecast, Guid userId);       
        PaginatedHistoryToResponseDto PaginateHistory(IEnumerable<History> history, HistoryRequestPaginationParameters historyRequestPaginationParameters);
    }
}