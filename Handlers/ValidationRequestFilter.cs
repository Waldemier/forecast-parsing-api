using System.Linq;
using System.Threading.Tasks;
using ForecastAPI.Data.Dtos;
using ForecastAPI.Data.Entities;
using ForecastAPI.Data.Enums;
using ForecastAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ForecastAPI.Handlers
{
    public class ValidationRequestFilter: IAsyncActionFilter
    {
        private readonly IUserRepository _userRepository;
        public ValidationRequestFilter(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var controller = context.RouteData.Values["controller"];
            var action = context.RouteData.Values["action"];

            // ["loginDto", LoginDto] => Key / Value
            dynamic param = context.ActionArguments.SingleOrDefault(x => x.Value.ToString().Contains("Dto")).Value;

            if (param == null)
            {
                context.Result = new BadRequestObjectResult($"Received object is null. Controller: {controller}, action: {action}");
                return;
            }

            if (!context.ModelState.IsValid)
            {
                context.Result = new UnprocessableEntityObjectResult(context.ModelState);
                return;
            }

            await next();
        }
    }
}