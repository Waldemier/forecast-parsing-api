using System;

namespace ForecastAPI.Data.Common
{
    public class Hour
    {
        public DateTime time { get; set; }
        public double temp_c { get; set; }
        public int cloud { get; set; }
        public int chance_of_rain { get; set; }
    }
}