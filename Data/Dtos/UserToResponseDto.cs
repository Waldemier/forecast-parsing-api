using System;
using System.Collections.Generic;
using ForecastAPI.Data.Entities;
using ForecastAPI.Data.Enums;

namespace ForecastAPI.Data.Dtos
{
    public class UserToResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public RoleTypes Role { get; set; }
        
        public IEnumerable<History> History { get; set; }
    }
}