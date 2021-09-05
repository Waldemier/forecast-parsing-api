using System;
using System.Collections.Generic;
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

        public static IQueryable<History> Sorting(this IQueryable<History> collection, string orderByQueryString)
        {
            if (orderByQueryString.Equals("Desc", StringComparison.InvariantCultureIgnoreCase))
            {
                return collection.OrderByDescending(x => x.Date);
            }
            return collection.OrderBy(x => x.Date);
        }
    }
}