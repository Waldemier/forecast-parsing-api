using System;

namespace ForecastAPI.Handlers
{
    public class RegisterConfirmationException: Exception
    {
        public RegisterConfirmationException()
        {
            
        }

        public RegisterConfirmationException(string message)
            :base(message)
        {
            
        }
        
        public RegisterConfirmationException(string message, Exception inner)
            :base(message, inner)
        {
            
        }
    }
}