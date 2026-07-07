using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Core;
using Domain.Exceptions.NotFoundExceptions;
using Shared.ErrorModels;

namespace LMS.Presentation.Middlewares
{
    public class GlobalErrorHandlingMiddleware(RequestDelegate _next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
                if(context.Response.StatusCode == StatusCodes.Status404NotFound)
                {
                    context.Response.ContentType = "application/json";
                    var response = new ErrorResponse
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        ErrorMessage = "The requested resource was not found."
                    };
                    await context.Response.WriteAsJsonAsync(response);
                }
            }
            catch(Exception ex)
            {
                context.Response.StatusCode = ex switch 
                {
                    NotFoundException => StatusCodes.Status404NotFound,
                    _ => StatusCodes.Status500InternalServerError
                };

                context.Response.ContentType = "application/json";

                var response = new ErrorResponse
                {
                  StatusCode = context.Response.StatusCode,
                  ErrorMessage = ex.Message
                };

                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}