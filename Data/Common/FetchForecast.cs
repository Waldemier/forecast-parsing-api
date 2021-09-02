using ForecastAPI.Data.Common;

namespace ForecastAPI.Data
{
    public class FetchForecast
    {
        public Location location { get; set; }
        public Current current { get; set; }
        public Forecast forecast { get; set; }
    }
}