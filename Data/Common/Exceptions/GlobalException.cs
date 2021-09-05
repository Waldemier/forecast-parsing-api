using Newtonsoft.Json;

namespace ForecastAPI.Data.Common.Exceptions
{
    internal class GlobalException
    {
        public string Action { get; }
        public object Controller { get; }
        public string StackTrace { get; }
        public string Message { get; }

        public GlobalException(string action, object controller, string stackTrace, string message)
        {
            Action = action;
            Controller = controller;
            StackTrace = stackTrace;
            Message = message;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}