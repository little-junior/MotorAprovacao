using MotorAprovacao.WebApi.ErrorHandlers;
using System.Text;

namespace MotorAprovacao.WebApi.Middlewares
{
    public class CustomHttpLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomHttpLoggingMiddleware> _logger;
        public CustomHttpLoggingMiddleware(RequestDelegate next, ILogger<CustomHttpLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            LogRequest(httpContext);

            await _next(httpContext);

            LogResponse(httpContext);
        }

        private void LogRequest(HttpContext context)
        {
            var request = context.Request;

            var requestLog = new StringBuilder();
            requestLog.AppendLine("Incoming Request:");
            requestLog.AppendLine($"HTTP {request.Method} {request.Path}");
            requestLog.AppendLine($"Host: {request.Host}");
            requestLog.AppendLine($"QueryString: {request.QueryString.ToString()}");
            requestLog.AppendLine($"Content-Type: {request.ContentType}");
            requestLog.AppendLine($"Content-Length: {request.ContentLength}");

            _logger.LogInformation(requestLog.ToString());
        }

        private void LogResponse(HttpContext context)
        {
            var response = context.Response;

            var responseLog = new StringBuilder();
            responseLog.AppendLine("Outgoing Response:");
            responseLog.AppendLine($"HTTP {response.StatusCode}");
            responseLog.AppendLine($"Content-Type: {response.ContentType}");
            responseLog.AppendLine($"Content-Length: {response.ContentLength}");

            _logger.LogInformation(responseLog.ToString());
        }
    }
}
