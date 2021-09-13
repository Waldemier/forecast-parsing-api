namespace ForecastAPI.Data.Dtos
{
    public class UsersRequestPaginationParameters: RequestPaginationParameters
    {
        public UsersRequestPaginationParameters() : 
            base(maxPageSize: 15, defaultPageSize: 6)
        {
        }
    }
}