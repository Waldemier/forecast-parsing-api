using ForecastAPI.Data.Enums;

namespace ForecastAPI.Data.Dtos
{
    public class UserToUpdateDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public RoleTypes Role { get; set; } = RoleTypes.SystemUser;
    }
}