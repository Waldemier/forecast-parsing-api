using AutoMapper;
using ForecastAPI.Data.Dtos;
using ForecastAPI.Data.Entities;

namespace ForecastAPI.Profiles
{
    public class HistoryProfile: Profile
    {
        public HistoryProfile()
        {
            CreateMap<History, HistoryToResponseDto>();
        }
    }
}