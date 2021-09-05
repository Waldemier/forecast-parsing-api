using System.Threading.Tasks;
using ForecastAPI.Data.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ForecastAPI.Handlers
{
    // To ask about production
    public class CustomExceptionFilter: ExceptionFilterAttribute // analogy is IAsyncExceptionFilter or IExceptionFilter
    {
        public override Task OnExceptionAsync(ExceptionContext context)
        {
            string actionName = context.ActionDescriptor.DisplayName;
            var controller = context.RouteData.Values["controller"];
            string exceptionStackTrace = context.Exception.StackTrace;
            string exceptionMessage = context.Exception.Message;

            var exception = new GlobalException(actionName, controller, exceptionStackTrace, exceptionMessage);
            context.Result = new JsonResult(exception);
            
            return Task.CompletedTask;
        }
    }
}