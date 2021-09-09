using System;

namespace ForecastAPI.Data.Dtos
{
    public class HistoryToResponseDto
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public double Temperature { get; set; }
        public string City { get; set; }
    }
}