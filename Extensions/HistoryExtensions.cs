using System;
using System.Linq;
using ForecastAPI.Data.Dtos;
using ForecastAPI.Data.Entities;
using System.Linq.Dynamic.Core;
using System.Reflection;

namespace ForecastAPI.Extensions
{
    public static class HistoryExtensions
    {
        private static string[] ORDERING_BY = new[] { "ASC", "DESC" };
        public static IQueryable<History> Filtering(this IQueryable<History> collection, RequestForHistoryDto requestForHistoryDto)
        {
            return collection
                .Where(x => x.Temperature <= requestForHistoryDto.Max && x.Temperature >= requestForHistoryDto.Min);
        }

        public static IQueryable<History> Sorting(this IQueryable<History> collection, string orderByQueryString)
        {
            orderByQueryString = ORDERING_BY.SingleOrDefault(
                x => x.Equals(orderByQueryString, StringComparison.InvariantCultureIgnoreCase)) ?? "ASC"; 
            
            var dateProperty = typeof(History).GetProperty("Date", BindingFlags.Instance | BindingFlags.Public);
            string orderingQuery = $"{dateProperty.Name} {orderByQueryString}";
            return collection.OrderBy(orderingQuery);
        }
    }
}