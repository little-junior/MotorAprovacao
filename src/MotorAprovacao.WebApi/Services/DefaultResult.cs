using Microsoft.AspNetCore.Mvc;
using MotorAprovacao.Models.Entities;

namespace MotorAprovacao.WebApi.Services
{
    public class DefaultResult
    {
        public DefaultResult()
        {
            Success = true;
        }

        public DefaultResult(IActionResult actionResult)
        {
            Success = false;
            ErrorActionResult = actionResult;
        }

        public bool Success { get; set; }
        public IActionResult? ErrorActionResult { get; set; }
    }

    public class DefaultResult<T>
    {
        public DefaultResult(T value)
        {
            Success = true;
            Value = value;
        }

        public DefaultResult(IActionResult actionResult)
        {
            Success = false;
            ErrorActionResult = actionResult;
        }

        public bool Success { get; set; }
        public T? Value { get; set; }
        public IActionResult? ErrorActionResult { get; set; }
    }
}
