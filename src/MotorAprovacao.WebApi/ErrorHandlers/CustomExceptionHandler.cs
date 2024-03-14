using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace MotorAprovacao.WebApi.ErrorHandlers
{
    public class CustomExceptionHandler : IExceptionHandler
    {
        public ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            httpContext.Response.StatusCode = 500;
            httpContext.Response.ContentType = "application/json";

            httpContext.Response.WriteAsJsonAsync(new ErrorResponse("500 - Internal Server Error", "An internal error occurred."));

            return ValueTask.FromResult(true);
        }
    }
}
