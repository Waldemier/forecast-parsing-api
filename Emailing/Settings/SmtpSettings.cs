namespace ForecastAPI.Emailing.Settings
{
    public class SmtpSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public Credentials Credentials { get; set; }
    }
}