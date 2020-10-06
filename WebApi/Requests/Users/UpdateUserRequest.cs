using FluentValidation;
using System;
using System.Collections.Generic;
using WebApi.Enums;

namespace WebApi.Requests.Users
{
    public class UpdateUserRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BusinessName { get; set; }
        public string Address { get; set; }
        public string PostCode { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public bool IsTaxablePerson { get; set; }
        public string VATNumber { get; set; }
        public string IBAN { get; set; }
        public List<Guid> Files { get; set; }
    }
}