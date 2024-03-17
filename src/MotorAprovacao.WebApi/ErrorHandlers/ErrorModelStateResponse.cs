using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MotorAprovacao.WebApi.ErrorHandlers
{
    public class ErrorModelStateResponse
    {
        public ErrorModelStateResponse(int statusCode, string type, ModelStateDictionary modelState) 
        {
            StatusCode = statusCode;
            Type = type;
            Messages = modelState
            .Where(entry => entry.Value.Errors.Any())
            .ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Errors.Select(error => error.ErrorMessage).ToArray()
            );
        }
        public int StatusCode { get; set; }
        public string Type { get; set; }
        public IDictionary<string, string[]> Messages { get; set; }
    }
}
