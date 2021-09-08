using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ForecastAPI.ActionFilters
{
    public class ValidationRequestsFilter: IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var controller = context.RouteData.Values["controller"];
            var action = context.ActionDescriptor.DisplayName;

            // ["loginDto", LoginDto] => Key / Value
            var param = context.ActionArguments.SingleOrDefault(x => x.Key.Contains("Dto")).Value;
            
            if (param is null)
            {
                context.Result = new BadRequestObjectResult($"Received object is null. Controller: {controller}, action: {action}");
                return;
            }

            if (!context.ModelState.IsValid)
            {
                context.Result = new UnprocessableEntityObjectResult(context.ModelState);
            }

            await next();
        }
    }
}