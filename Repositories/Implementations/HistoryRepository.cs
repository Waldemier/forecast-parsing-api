using System.Collections.Generic;
using System.Threading.Tasks;
using ForecastAPI.Data;
using ForecastAPI.Data.Common.Pagination;
using ForecastAPI.Data.Dtos;
using ForecastAPI.Data.Entities;
using ForecastAPI.Repositories.Interfaces;

namespace ForecastAPI.Repositories.Implementations
{
    public class HistoryRepository: BaseRepository<History>, IHistoryRepository
    {
        public HistoryRepository(ApplicationDbContext context) : 
            base(context)
        {
        }

        public PagedList<History> PaginateHistory(IEnumerable<History> history, HistoryRequestPaginationParameters historyRequestPaginationParameters)
        {
            return PagedList<History>.ToPagedList(history, historyRequestPaginationParameters.PageNumber,
                historyRequestPaginationParameters.PageSize);
        }
    }
}