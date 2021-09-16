using System;
using System.Threading.Tasks;
using ForecastAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ForecastAPI.Handlers
{
    public class ValidationUserExistingFilter: IAsyncActionFilter
    {
        private readonly IUserRepository _userRepository;

        public ValidationUserExistingFilter(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var controller = context.RouteData.Values["controller"];
            var action = context.RouteData.Values["action"];

            var userId = (Guid)context.ActionArguments["userId"];
            var userInstance = await _userRepository.GetInstanceById(userId);

            if (userInstance is null)
            {
                context.Result = new NotFoundObjectResult($"User with {userId} doesn't exist. Controller: {controller}, action {action}");
                return;
            }
            
            await next();
        }
    }
}