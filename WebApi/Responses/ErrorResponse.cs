using System.Collections.Generic;

namespace WebApi.Responses
{
    public class ErrorResponse
    {
        public object Data { get; set; }
        public List<Error> Errors { get; set; }
    }

    public class Error
    {
        public string ErrorMessage { get; set; }
        public string FieldName { get; set; }
    }
}
