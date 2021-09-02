using System.Collections.Generic;

namespace ForecastAPI.Data.Common
{
    public class Forecast
    {
        public ICollection<ForecastDay> forecastday { get; set; }
    }
}