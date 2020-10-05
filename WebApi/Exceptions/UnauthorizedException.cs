using System;
using System.Net;

namespace WebApi.Exceptions
{
    public class UnauthorizedException : RequestException
    {
        public UnauthorizedException()
            : base()
        {
            StatusCode = HttpStatusCode.Unauthorized;
        }

        public UnauthorizedException(string message)
            : base(message)
        {
            StatusCode = HttpStatusCode.Unauthorized;
        }

        public UnauthorizedException(string message, Exception inner)
            : base(message, inner)
        {
            StatusCode = HttpStatusCode.Unauthorized;
        }
    }
}
