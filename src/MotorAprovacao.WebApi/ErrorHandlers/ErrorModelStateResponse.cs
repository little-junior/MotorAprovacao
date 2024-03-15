using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MotorAprovacao.WebApi.ErrorHandlers
{
    public class ErrorModelStateResponse
    {
        public ErrorModelStateResponse(string statusCode, ModelStateDictionary messages) 
        {
            StatusCode = statusCode;
            Messages = messages;
        }
        public string StatusCode { get; set; }
        public ModelStateDictionary Messages { get; set; }
    }
}
