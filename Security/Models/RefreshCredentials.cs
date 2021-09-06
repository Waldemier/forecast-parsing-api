namespace ForecastAPI.Security.Models
{
    public class RefreshCredentials
    {
        public string JwtToken { get; set; }
        public string RefreshToken { get; set; }
    }
}