namespace ForecastAPI.Data.Dtos
{
    public class HistoryRequestPaginationParameters: RequestPaginationParameters
    {
        public HistoryRequestPaginationParameters(): 
            base(maxPageSize: 25, defaultPageSize: 7)
        {
        }
    }
}