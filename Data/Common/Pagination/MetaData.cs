using Newtonsoft.Json;

namespace ForecastAPI.Data.Common.Pagination
{
    public class MetaData
    {
        public int TotalItemsAmount { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }

        public bool HasNextPage => CurrentPage < TotalPages;
        
        public bool HasPreviousPage => CurrentPage > 1;

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}