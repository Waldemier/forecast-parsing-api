using System;

namespace ForecastAPI.Handlers
{
    public class CustomAdminException : Exception
    {
        public CustomAdminException()
        {
        }

        public CustomAdminException(string message)
            : base(message)
        {
        }

        public CustomAdminException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}