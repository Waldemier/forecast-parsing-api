namespace ForecastAPI.Emailing.Models
{
    public class ForgotLetter
    {
        public ForgotLetter(string verifyToken)
        {
            VerifyToken = verifyToken;
        }

        public string VerifyToken { get; set; }
    }
}