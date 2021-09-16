using System.Collections.Generic;

namespace ForecastAPI.Data.Dtos
{
    public class PaginatedHistoryToResponseDto
    {
        public IEnumerable<HistoryToResponseDto> HistoryToResponseDtos { get; set; }
        public string MetaData { get; set; }
    }
}