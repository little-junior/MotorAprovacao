using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MotorAprovacao.WebApi.ErrorHandlers;

namespace MotorAprovacao.WebApi.Filters
{
    public class ValidationActionFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                //To do: formatar como o resto das mensagens de erro
                context.Result = new BadRequestObjectResult(context.ModelState);
            }
        }
    }
}
