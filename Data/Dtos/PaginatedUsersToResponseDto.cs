using System.Collections.Generic;

namespace ForecastAPI.Data.Dtos
{
    public class PaginatedUsersToResponseDto
    {
        public IEnumerable<UserToResponseDto> UsersToResponseDtos { get; set; }
        public string MetaData { get; set; }
    }
}