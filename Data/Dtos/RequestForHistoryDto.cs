using System;

namespace ForecastAPI.Data.Dtos
{
    public class RequestForHistoryDto
    {
        public double Max { get; set; } = 100;
        public double Min { get; set; } = -100;
        public string OrderBy { get; set; } = "Asc";
    }
}