namespace MotorAprovacao.WebApi.ErrorHandlers
{
    public class ErrorResponse
    {
        public ErrorResponse(int statusCode, string type, string message) 
        {
            StatusCode = statusCode;
            Type = type;
            Message = message;
        }

        public int StatusCode { get; set; }
        public string Type { get; set; }
        public string Message { get; set; }
    }
}
