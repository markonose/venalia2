using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Responses;

namespace WebApi.Filters
{
    public class ValidationFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //var requestHasMetadata = context.HttpContext.Request.Path.StartsWithSegments(new Microsoft.AspNetCore.Http.PathString("/api/installations"));

            if (!context.ModelState.IsValid)
            {
                var errorResponse = new ErrorResponse()
                {
                    Errors = new List<Error>()
                };

                var invalidFields = context.ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(x => x.ErrorMessage))
                    .ToArray();

                foreach (var invalidField in invalidFields)
                {
                    foreach (var error in invalidField.Value)
                    {
                        errorResponse.Errors.Add(new Error()
                        {
                            FieldName = $"{invalidField.Key}",
                            ErrorMessage = $"{error}"
                        });
                    }
                }

                context.Result = new BadRequestObjectResult(errorResponse);

                return;
            }

            await next();
        }
    }
}
