using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MotorAprovacao.WebApi.ErrorHandlers
{
    public class ErrorModelStateResponse
    {
        public ErrorModelStateResponse(string statusCode, ModelStateDictionary modelState) 
        {
            StatusCode = statusCode;
            Messages = modelState
            .Where(entry => entry.Value.Errors.Any())
            .ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Errors.Select(error => error.ErrorMessage).ToArray()
            );
        }
        public string StatusCode { get; set; }
        public IDictionary<string, string[]> Messages { get; set; }
    }
}
