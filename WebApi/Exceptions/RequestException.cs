using System;
using System.Net;

namespace WebApi.Exceptions
{
    public abstract class RequestException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }

        protected RequestException()
            : base()
        {
        }

        protected RequestException(string message)
            : base(message)
        {
        }

        protected RequestException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
