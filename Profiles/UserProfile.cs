using AutoMapper;
using ForecastAPI.Data.Dtos;
using ForecastAPI.Data.Entities;

namespace ForecastAPI.Profiles
{
    public class UserProfile: Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserToResponseDto>();
            CreateMap<UserToUpdateDto, User>();
        }
    }
}