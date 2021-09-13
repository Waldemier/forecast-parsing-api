namespace ForecastAPI.Data.Dtos
{
    public abstract class RequestPaginationParameters
    {
        private readonly int _maxPageSize;
        private int _defaultPageSize;
        private int _defaultPageNumber = 1;
        
        protected RequestPaginationParameters(int maxPageSize, int defaultPageSize)
        {
            _maxPageSize = maxPageSize;
            _defaultPageSize = defaultPageSize;
        }
        
        public int PageSize
        {
            get => _defaultPageSize;
            set => _defaultPageSize = (value <= _maxPageSize && value > 0) ? value : _maxPageSize;
        }
        
        public int PageNumber
        {
            get => _defaultPageNumber; 
            set => _defaultPageNumber = (value > 0) ? value : 1;
        }
    }
}