namespace ForecastAPI.Emailing.Settings
{
    public class FluentEmailSettings
    {
        public string From { get; set; }
        public SmtpSettings SmtpSettings { get; set; }
    }
}