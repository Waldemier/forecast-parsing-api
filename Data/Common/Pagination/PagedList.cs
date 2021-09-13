using System;
using System.Collections.Generic;
using System.Linq;

namespace ForecastAPI.Data.Common.Pagination
{
    public class PagedList<T> : List<T> where T: class
    {
        public string MetaData { get; }
        private PagedList(IEnumerable<T> items, int itemsAmount, int pageNumber, int pageSize)
        {
            MetaData = new MetaData()
            {
                TotalItemsAmount = itemsAmount,
                CurrentPage = pageNumber,
                PageSize = pageSize,
                // for example: 7.03 => 8
                TotalPages = (int)Math.Ceiling(itemsAmount / (decimal)pageSize)
            }.ToString();
                
            base.AddRange(items);
        }

        public static PagedList<T> ToPagedList(IEnumerable<T> sourceCollection, int pageNumber, int pageSize)
        {
            var collection = sourceCollection.ToList();
            var itemsAmount = collection.Count();
            // if will we get some negative number, that do nothing wrong with formula below
            var itemsPerPage = collection.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            return new PagedList<T>(itemsPerPage, itemsAmount, pageNumber, pageSize);
        }   
    }
}