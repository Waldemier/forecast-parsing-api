using System;
using System.Collections.Generic;

namespace ForecastAPI.Data.Common
{
    public class ForecastDay
    {
        public DateTime date { get; set; }
        public Day day { get; set; }
        public ICollection<Hour> hour { get; set; }
    }
}