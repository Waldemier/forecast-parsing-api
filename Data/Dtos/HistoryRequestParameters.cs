namespace ForecastAPI.Data.Dtos
{
    public class HistoryRequestParameters: RequestParameters
    {
        private const int MAX_PAGE_SIZE = 25;

        private int _defaultPageSize = 7;
        
        private int _defaultPageNumber = 1;
        
        public override int PageSize
        {
            get => _defaultPageSize;
            set => _defaultPageSize = (value <= MAX_PAGE_SIZE && value > 0) ? value : MAX_PAGE_SIZE;
        }
        
        public override int PageNumber
        {
            get => _defaultPageNumber; 
            set => _defaultPageNumber = (value > 0) ? value : 1;
        }
    }
}