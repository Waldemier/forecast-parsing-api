using System;

namespace ForecastAPI.Data.Common
{
    public class Current
    {
        public DateTime last_updated { get; set; }
        public double temp_c { get; set; }
    }
}