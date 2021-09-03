using System;

namespace ForecastAPI.Data.Dtos
{
    public class RequestForHistoryDto
    {
        public double Max { get; set; } = Double.MaxValue;
        public double Min { get; set; } = Double.MinValue;
    }
}