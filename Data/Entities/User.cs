using System;
using System.Collections.Generic;
using System.Linq;
using ForecastAPI.Data.Enums;

namespace ForecastAPI.Data.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public RoleTypes Role { get; set; }

        public IEnumerable<History> History { get; set; }
        public RegisterConfirm RegisterConfirm { get; set; }
        public VerifyPassword VerifyPassword { get; set; }
    }
}