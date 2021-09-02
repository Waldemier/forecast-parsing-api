using System;

namespace ForecastAPI.Data.Common.Settings
{
    public class ForecastSettings
    {
        public string Key { get; set; }
        public string Url { get; set; }
        public string City { get; set; }
        public int Days { get; set; }
    }
}