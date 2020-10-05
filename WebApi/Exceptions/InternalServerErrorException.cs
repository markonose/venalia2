using System;
using System.Net;

namespace WebApi.Exceptions
{
    public class InternalServerErrorException : RequestException
    {
        public InternalServerErrorException()
            : base()
        {
            StatusCode = HttpStatusCode.InternalServerError;
        }

        public InternalServerErrorException(string message)
            : base(message)
        {
            StatusCode = HttpStatusCode.InternalServerError;
        }

        public InternalServerErrorException(string message, Exception inner)
            : base(message, inner)
        {
            StatusCode = HttpStatusCode.InternalServerError;
        }
    }
}
