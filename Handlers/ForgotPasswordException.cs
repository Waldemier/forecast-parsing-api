using System;

namespace ForecastAPI.Handlers
{
    public class ForgotPasswordException: Exception
    {
        public ForgotPasswordException()
        {
            
        }

        public ForgotPasswordException(string message)
            :base(message)
        {
            
        }
        
        public ForgotPasswordException(string message, Exception inner)
            :base(message, inner)
        {
            
        }
    }
}