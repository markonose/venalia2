using System;
using System.Collections.Generic;
using WebApi.Enums;

namespace WebApi.Responses.Users
{
    public class GetContextUserResponse
    {
        public GetContextUserResponseData Data { get; set; }
        public List<Error> Errors { get; set; }
    }

    public class GetContextUserResponseData
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public UserType Type { get; set; }
        public UserStatus Status { get; set; }
    }
}
