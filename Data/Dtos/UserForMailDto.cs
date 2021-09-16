using System;

namespace ForecastAPI.Data.Dtos
{
    public class UserForMailDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}