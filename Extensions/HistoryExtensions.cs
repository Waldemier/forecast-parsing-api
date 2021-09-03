using System.Linq;
using ForecastAPI.Data.Dtos;
using ForecastAPI.Data.Entities;

namespace ForecastAPI.Extensions
{
    public static class HistoryExtensions
    {
        public static IQueryable<History> Filtering(this IQueryable<History> collection, RequestForHistoryDto requestForHistoryDto)
        {
            return collection
                .Where(x => x.Temperature <= requestForHistoryDto.Max && x.Temperature >= requestForHistoryDto.Min);
        }
    }
}