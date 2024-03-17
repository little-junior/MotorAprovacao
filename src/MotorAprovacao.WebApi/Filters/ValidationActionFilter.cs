﻿using Microsoft.AspNetCore.Mvc;
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
                var errorResponse = new ErrorModelStateResponse(400, "Bad Request", context.ModelState);

                context.Result = new BadRequestObjectResult(errorResponse);
            }
        }
    }
}
