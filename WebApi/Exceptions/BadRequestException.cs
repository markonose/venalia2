using System;
using System.Net;

namespace WebApi.Exceptions
{
    public class BadRequestException : RequestException
    {
        public BadRequestException()
            : base()
        {
            StatusCode = HttpStatusCode.BadRequest;
        }

        public BadRequestException(string message)
            : base(message)
        {
            StatusCode = HttpStatusCode.BadRequest;
        }

        public BadRequestException(string message, Exception inner)
            : base(message, inner)
        {
            StatusCode = HttpStatusCode.BadRequest;
        }
    }
}
