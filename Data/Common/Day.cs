using System.Collections.Generic;

namespace ForecastAPI.Data.Common
{
    /// <summary>
    /// daily_chance_of_rain is the percentage property
    /// </summary>
    public class Day
    {
        public double maxtemp_c { get; set; }
        public double mintemp_c { get; set; }
        public int daily_chance_of_rain { get; set; }
        
    }
}