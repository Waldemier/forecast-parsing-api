namespace ForecastAPI.Data.Dtos
{
    public abstract class RequestParameters
    {
        private const int MAX_PAGE_SIZE = 15;

        private int _defaultPageSize = 6;
        
        private int _defaultPageNumber = 1;
        
        public virtual int PageSize
        {
            get => _defaultPageSize;
            set => _defaultPageSize = (value <= MAX_PAGE_SIZE && value > 0) ? value : MAX_PAGE_SIZE;
        }
        
        public virtual int PageNumber
        {
            get => _defaultPageNumber; 
            set => _defaultPageNumber = (value > 0) ? value : 1;
        }
    }
}