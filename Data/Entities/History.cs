using System;

namespace ForecastAPI.Data.Entities
{
    public class History
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public double Temperature { get; set; }
        public string City { get; set; }
        
        public Guid UserId { get; set; }
        
        public User User { get; set; }

        public History() { }
        
        public History(DateTime date, double temperature, string city, Guid userId)
        {
            this.Date = date;
            this.Temperature = temperature;
            this.City = city;
            this.UserId = userId;
        }
    }
}