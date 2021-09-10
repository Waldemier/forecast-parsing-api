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
        private readonly ApplicationDbContext _context;
        public HistoryRepository(ApplicationDbContext context) : 
            base(context)
        {
            _context = context;
        }


        public PagedList<History> PaginateHistory(IEnumerable<History> history, HistoryRequestParameters historyRequestParameters)
        {
            return PagedList<History>.ToPagedList(history, historyRequestParameters.PageNumber,
                historyRequestParameters.PageSize);
        }
    }
}