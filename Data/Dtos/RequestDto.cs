using System.Globalization;

namespace ForecastAPI.Data.Dtos
{
    public class RequestDto
    {
        public string City { get; set; } = RegionInfo.CurrentRegion.EnglishName;
        public int Days { get; set; } = 2;
    }
}