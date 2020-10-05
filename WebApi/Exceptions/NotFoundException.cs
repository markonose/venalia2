using System;
using System.Net;

namespace WebApi.Exceptions
{
    public class NotFoundException : RequestException
    {
        public NotFoundException()
            : base()
        {
            StatusCode = HttpStatusCode.NotFound;
        }

        public NotFoundException(string message)
            : base(message)
        {
            StatusCode = HttpStatusCode.NotFound;
        }

        public NotFoundException(string message, Exception inner)
            : base(message, inner)
        {
            StatusCode = HttpStatusCode.NotFound;
        }
    }
}
