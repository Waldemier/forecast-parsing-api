namespace ForecastAPI.Emailing.Models
{
    public class RegistrationLetter
    {
        public string UserName { get; set; }
        public string VerifyLink { get; set; }
        public RegistrationLetter(string userName, string verifyLink)
        {
            UserName = userName;
            VerifyLink = verifyLink;
        }
    }
}