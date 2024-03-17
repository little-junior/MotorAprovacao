using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MotorAprovacao.WebApi.ErrorHandlers;

namespace MotorAprovacao.WebApi.Filters
{
    public class ValidationActionFilter : IActionFilter
    {
        private readonly ILogger<ValidationActionFilter> _logger;
        public ValidationActionFilter(ILogger<ValidationActionFilter> logger)
        {
            _logger = logger;
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                _logger.LogWarning("Request ended because the model state is invalid");

                var errorResponse = new ErrorResponse(400, "Bad Request", context.ModelState);

                context.Result = new BadRequestObjectResult(errorResponse);
            }
        }
    }
}
