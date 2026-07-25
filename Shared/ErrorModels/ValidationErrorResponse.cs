using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Shared.ErrorModels
{
    public class ValidationErrorResponse : ErrorResponse
    {
        public IEnumerable<ValidationMessage> Errors { get; set; } = null!;
        public ValidationErrorResponse()
        {
            StatusCode = StatusCodes.Status400BadRequest;
            ErrorMessage = "Validation Error";
        }
    }

    public class ValidationMessage 
    {
        public string Field { get; set; } = null!;
        public IEnumerable<string> Errors { get; set; } = null!;
    }
}