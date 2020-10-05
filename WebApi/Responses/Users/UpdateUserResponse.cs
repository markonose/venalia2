using System;
using System.Collections.Generic;

namespace WebApi.Responses.Users
{
    public class UpdateUserResponse
    {
        public UpdateUserResponseData Data { get; set; }
        public List<Error> Errors { get; set; }
    }

    public class UpdateUserResponseData
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BusinessName { get; set; }
        public string Address { get; set; }
        public string PostCode { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string VATNumber { get; set; }
        public string IBAN { get; set; }
    }
}
