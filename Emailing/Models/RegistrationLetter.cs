namespace ForecastAPI.Emailing.Models
{
    public class RegistrationLetter
    {
        public string Name { get; set; }
        public string VerifyLink { get; set; }
        public RegistrationLetter(string name, string verifyLink)
        {
            Name = name;
            VerifyLink = verifyLink;
        }
    }
}