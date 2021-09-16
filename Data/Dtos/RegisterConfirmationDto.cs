using System;

namespace ForecastAPI.Data.Dtos
{
    public class RegisterConfirmationDto
    {
        public Guid UserId { get; set; }
        public bool HasExpired { get; set; }
    }
}