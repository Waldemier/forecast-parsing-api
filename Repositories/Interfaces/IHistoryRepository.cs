using System.Collections.Generic;
using System.Threading.Tasks;
using ForecastAPI.Data.Common.Pagination;
using ForecastAPI.Data.Dtos;
using ForecastAPI.Data.Entities;

namespace ForecastAPI.Repositories.Interfaces
{
    public interface IHistoryRepository: IBaseRepository<History>
    {
        PagedList<History> PaginateHistory(IEnumerable<History> history, HistoryRequestParameters historyRequestParameters);
    }
}