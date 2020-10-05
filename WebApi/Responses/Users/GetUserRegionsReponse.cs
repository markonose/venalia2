using System;
using System.Collections.Generic;

namespace WebApi.Responses.Users
{
    public class GetUserRegionsResponse
    {
        public Dictionary<string, string> Data { get; set; }

        public List<Error> Errors { get; set; }
    }
}
