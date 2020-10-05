using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using WebApi.Exceptions;
using WebApi.Responses;

namespace WebApi.Filters
{
    public class ExceptionFilter : IAsyncExceptionFilter
    {
        public Task OnExceptionAsync(ExceptionContext context)
        {
            string errorMessage;
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;

            if (context.Exception is RequestException)
            {
                var requestException = context.Exception as RequestException;

                errorMessage = requestException.Message;
                statusCode = requestException.StatusCode;
            }

            context.ExceptionHandled = true; // mark exception as handled
            context.HttpContext.Response.StatusCode = (int) statusCode;
            context.Result = new ObjectResult(
                new ErrorResponse()
                {
                    Errors = new List<Error>()
                    {
                        new Error()
                        {
                            ErrorMessage = context.Exception.Message
                        }
                    }
                });

            return Task.CompletedTask;
        }
    }
}
