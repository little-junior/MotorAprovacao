namespace MotorAprovacao.WebApi.ErrorHandlers
{
    public class ErrorResponse
    {
        public ErrorResponse(string statusCode, string message) 
        {
            StatusCode = statusCode;
            Message = message;
        }

        public string StatusCode { get; set; }
        public string Message { get; set; }
    }
}
